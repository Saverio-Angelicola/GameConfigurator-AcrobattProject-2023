using System;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.AccountTests;

public class LoginAccountTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task LoginToTheirAccount()
    {
        //Arrange
        using var scope = _app.Services.CreateScope();
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        RegisterAccount registerCommand = new("saverio", "angelicola", "sav", "angelicola.saverio@gmail.com", "Toto!1234");
        await commandDispatcher.DispatchAsync<RegisterAccount, RegisterAccountResult>(registerCommand, CancellationToken.None);
        
        //Act
        LoginAccount command = new("angelicola.saverio@gmail.com", "Toto!1234");
        LoginAccountResult result = await commandDispatcher.DispatchAsync<LoginAccount, LoginAccountResult>(command, CancellationToken.None);
        
        //Assert
        result.Email.Should().Be("angelicola.saverio@gmail.com");
        result.Token.Should().NotBeNullOrEmpty();
    }
}