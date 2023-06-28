using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.GameConfigurations;

public class GetPrivateGameConfigurationsTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task StatusCodeSuccessWhenGetPrivateGameConfigurations()
    {
        IServiceScope scope = _app.Services.CreateScope();
        LoginAccountResult credentials = await CredentialsUtils.GetCredentials(scope, "angelicola.saverio@gmail.com");
        HttpClient client = _app.CreateClient();
        await GameConfigurationUtils.CreateGameConfiguration(scope, client, credentials, true);
        HttpResponseMessage result = await client.GetAsync("/GameConfiguration/private");
        List<PrivateGameConfigurationViewModel>? privateGameConfiguration =
            JsonSerializer.Deserialize<List<PrivateGameConfigurationViewModel>>(await result.Content.ReadAsStringAsync());
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        privateGameConfiguration?.Count.Should().Be(1);
    }
}