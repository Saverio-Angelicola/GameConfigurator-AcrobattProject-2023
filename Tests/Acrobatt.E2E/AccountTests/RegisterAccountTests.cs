using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.AccountTests;

public class RegisterAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task StatusCodeSuccessWhenRegisterAccount()
    {
        await CredentialsUtils.CreateAccount(_app.Services.CreateScope(), "angelicola.saverio@gmail.com");
        HttpClient client = _app.CreateClient();
        string body = JsonSerializer.Serialize(new RegisterAccount("saverio", "angelicola", "sav", "sav@gmail.com", "Toto!1234"));
        HttpResponseMessage response = await client.PostAsync("/Authentication/Register", new StringContent(body, Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}