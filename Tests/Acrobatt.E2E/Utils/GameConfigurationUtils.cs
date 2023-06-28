using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;
using Acrobatt.Domain.Accounts.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Acrobatt.E2E.Utils;

public static class GameConfigurationUtils
{
    public static async Task CreateGameConfiguration(IServiceScope scope, HttpClient client, LoginAccountResult credentials, bool isPrivate)
    {

        CreateGameConfigurationDto dto = new("game 1",4,5,10000,GameMode.Flag, isPrivate, new List<TeamDto>()
        {
            new("#fff", 5, "team 1"),
            new("#eee", 5, "team 2")
        }, "map1", true,new MapCenterDto(25, 24));
        
        byte[] byteArray = "jsonfile"u8.ToArray();
        Stream stream = new MemoryStream(byteArray);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/GameConfiguration/Create");
        MultipartFormDataContent content = new MultipartFormDataContent
        {
            { new StreamContent(stream), "mapFile", "map_config.json" },
        };
        
        content.Add(new StringContent(dto.GameName), "gameName");
        content.Add(new StringContent(dto.MaxPlayers.ToString()), "maxPlayers");
        content.Add(new StringContent(dto.MaxPlayers.ToString()), "maxFlags");
        content.Add(new StringContent(dto.Duration.ToString(CultureInfo.InvariantCulture)), "duration");
        content.Add(new StringContent(dto.GameMode.ToString()), "gameMode");
        content.Add(new StringContent(dto.IsPrivate.ToString()), "isPrivate");
        content.Add(new StringContent(dto.MapName), "mapName");
        content.Add(new StringContent(dto.MapVisibility.ToString()), "mapVisibility");
        content.Add(new StringContent(dto.Teams[0].Color), "teams[0][color]");
        content.Add(new StringContent(dto.Teams[0].NbPlayer.ToString()), "teams[0][nbPlayer]");
        content.Add(new StringContent(dto.Teams[0].Name), "teams[0][name]");
        content.Add(new StringContent(dto.Teams[1].Color), "teams[1][color]");
        content.Add(new StringContent(dto.Teams[1].NbPlayer.ToString()), "teams[1][nbPlayer]");
        content.Add(new StringContent(dto.Teams[1].Name), "teams[1][name]");
        content.Add(new StringContent(dto.MapCenter.X.ToString(CultureInfo.CurrentCulture)), "mapCenter[x]");
        content.Add(new StringContent(dto.MapCenter.Y.ToString(CultureInfo.InvariantCulture)), "mapCenter[y]");
        
        request.Content = content;
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + credentials.Token);
        await client.SendAsync(request);
    }
}