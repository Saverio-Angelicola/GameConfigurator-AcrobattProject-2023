using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.E2E.Configs;
using Acrobatt.E2E.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.E2E.AccountTests;

public class DeleteAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();
    
    [Test]
    public async Task CodeStatusSuccessAfterDeleteAccount()
    {
        using IServiceScope scope = _app.Services.CreateScope();
        LoginAccountResult credentials = await CredentialsUtils.GetCredentials(scope, "angelicola.saverio@gmail.com");
        HttpClient client = _app.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + credentials.Token);
        HttpResponseMessage response = await client.DeleteAsync("/Account");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}