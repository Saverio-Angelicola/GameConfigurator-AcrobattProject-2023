using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.AccountTests;

public class UpdateAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task CodeStatusSuccessAfterUpdateAccount()
    {
        using IServiceScope scope = _app.Services.CreateScope();
        LoginAccountResult credentials = await CredentialsUtils.GetCredentials(scope, "angelicola.saverio@gmail.com");
        HttpClient client = _app.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + credentials.Token);
        string body = JsonSerializer.Serialize(new UpdateAccount("mark", "mark", "mark3", "mark@gmail.com"));
        HttpResponseMessage response = await client.PatchAsync("/Account", new StringContent(body, Encoding.UTF8, "application/json"));
        UpdateAccountResult? result = JsonSerializer.Deserialize<UpdateAccountResult>(await response.Content.ReadAsStringAsync());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result?.Email.Should().NotBe(credentials.Email);
    }
}