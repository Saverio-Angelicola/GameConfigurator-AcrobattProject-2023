using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.GameConfigurations;

public class GetPublicGameConfigurationsTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task StatusCodeSuccessWhenGetPrivateGameConfigurations()
    {
        // Arrange
        IServiceScope scope = _app.Services.CreateScope();
        LoginAccountResult credentials1 = await CredentialsUtils.GetCredentials(scope, "otheruser@gmail.com");
        LoginAccountResult credentials2 = await CredentialsUtils.GetCredentials(scope, "angelicola.saverio@gmail.com");
        HttpClient client1 = _app.CreateClient();
        await GameConfigurationUtils.CreateGameConfiguration(scope, client1, credentials2, false);
        
        
        // Act
        HttpClient client2 = _app.CreateClient();
        client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + credentials1.Token);
        HttpResponseMessage result = await client2.GetAsync("/GameConfiguration/public");
        List<PublicGameConfigurationViewModel>? privateGameConfiguration =
            JsonSerializer.Deserialize<List<PublicGameConfigurationViewModel>>(await result.Content.ReadAsStringAsync());
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        privateGameConfiguration?.Count.Should().Be(1);
    }
}