using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.Entities;
using Acrobatt.Domain.Accounts.Enums;
using Acrobatt.Domain.Accounts.ValueObjects;
using Acrobatt.Domain.Gateways;
using Acrobatt.IntegrationTests.Configs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acrobatt.IntegrationTests.GameConfigurationsTests;

public class GetPrivateGameConfigurationsTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task ShowPrivateGameConfigurationList()
    {
        using IServiceScope scope = _app.Services.CreateScope();
        IQueryDispatcher queryDispatcher = (IQueryDispatcher)scope.ServiceProvider.GetRequiredService(typeof(IQueryDispatcher));
        await CreateDataBeforeTest(scope);
        GetPrivateGameConfigurationsQuery command = new();
        List<PrivateGameConfigurationViewModel> result = await queryDispatcher.DispatchAsync<GetPrivateGameConfigurationsQuery, List<PrivateGameConfigurationViewModel>>(command, CancellationToken.None);
        result.Count.Should().Be(2);
        result.First().GameName.Should().Be("partie1");
    }

    private async Task CreateDataBeforeTest(IServiceScope scope)
    {
        IAccountRepository accountRepository =
            (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IAuthenticationGateway authenticationGateway =
            (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola",
            "angelicola.saverio@gmail.com", "Toto!1234");
        account.AddGameConfiguration(GameConfiguration.Create("partie1", 6, 3, 600, GameMode.Flag, false,
            new Map("map1", false, new Position(2, 2)), new List<Team>()
            {
                new(1, "#fff", 3, "team1"),
                new(2, "#efef", 3, "team2")
            }));
        account.AddGameConfiguration(GameConfiguration.Create("partie2", 7, 5, 500, GameMode.Flag, true,
            new Map("map2", false, new Position(2, 2)), new List<Team>()
            {
                new(3, "#fff", 3, "team1"),
                new(4, "#efef", 4, "team2")
            }));
        await accountRepository.AddAsync(account, CancellationToken.None);
        authenticationGateway.Authenticate(account);
    }
}