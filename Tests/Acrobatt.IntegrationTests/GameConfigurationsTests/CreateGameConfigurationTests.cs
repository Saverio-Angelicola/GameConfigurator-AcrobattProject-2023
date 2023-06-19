using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.LoginAccount;
using Acrobatt.Application.Accounts.Features.RegisterAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.GameConfigurations.Features.CreateGameConfiguration;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.GameConfigurationsTests;

public class CreateGameConfigurationTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task CreateNewGameConfiguration()
    {
        //Arrange
        using var scope = _app.Services.CreateScope();
        ICommandDispatcher commandDispatcher = (ICommandDispatcher)scope.ServiceProvider.GetRequiredService(typeof(ICommandDispatcher));
        IAccountRepository accountRepository =
            (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IAuthenticationGateway authenticationGateway = (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola", "angelicola.saverio@gmail.com", "Toto!1234");

        await accountRepository.AddAsync(account, CancellationToken.None);
        authenticationGateway.Authenticate(account);
        //Act
        using MemoryStream stream = new();
        CreateGameConfiguration command = new(stream, new CreateGameConfigurationDto("mapartie", 4, 5, 600, GameMode.Supremacy, true, new List<TeamDto>()
        {
            new TeamDto("#fff", 2, "team alpha"),
            new TeamDto("#aaa", 2, "team beta")
        }, "ma-map", false, new MapCenterDto(2,2)));
        CreateGameConfigurationResult result = await commandDispatcher.DispatchAsync<CreateGameConfiguration, CreateGameConfigurationResult>(command, CancellationToken.None);
        
        //Assert
        result.GameConfigurationId.Should().Be(1);
        account.GameConfigurations.Count.Should().Be(1);
        account.GameConfigurations.First().GameName.Should().Be("mapartie");
    }
}