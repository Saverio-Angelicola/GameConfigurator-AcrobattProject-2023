using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.GameConfigurations;

public class GameConfigurationDetailsTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task StatusCodeSuccessWhenGetGameConfigurationDetail()
    {
        IServiceScope scope = _app.Services.CreateScope();
        LoginAccountResult credentials = await CredentialsUtils.GetCredentials(scope, "angelicola.saverio@gmail.com");
        HttpClient client = _app.CreateClient();
        await GameConfigurationUtils.CreateGameConfiguration(scope, client, credentials, true);
        HttpResponseMessage result = await client.GetAsync($"GameConfiguration/{1}");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}