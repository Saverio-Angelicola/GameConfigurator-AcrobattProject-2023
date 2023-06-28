using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.GameConfigurations.Features.GetPrivateGameConfigurations;
using Acrobatt.Application.GameConfigurations.Features.GetPublicGameConfigurations;
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

public class GetPublicGameConfigurationsTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task ShowPublicGameConfigurationList()
    {
        using IServiceScope scope = _app.Services.CreateScope();
        IQueryDispatcher queryDispatcher = (IQueryDispatcher)scope.ServiceProvider.GetRequiredService(typeof(IQueryDispatcher));
        await CreateDataBeforeTest(scope);
        GetPublicGameConfigurationsQuery command = new();
        List<PublicGameConfigurationViewModel> result = await queryDispatcher.DispatchAsync<GetPublicGameConfigurationsQuery, List<PublicGameConfigurationViewModel>>(command, CancellationToken.None);
        result.Count.Should().Be(1);
        result.First().GameName.Should().Be("partie de Salvo");
    }

    private async Task CreateDataBeforeTest(IServiceScope scope)
    {
        IAccountRepository accountRepository =
            (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IAuthenticationGateway authenticationGateway =
            (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola",
            "angelicola.saverio@gmail.com", "Toto!1234");
        account.AddGameConfiguration(GameConfiguration.Create("partie de sav 1", 6, 3, 600, GameMode.Flag, false,
            new Map("map1", false, new Position(2, 2)), new List<Team>()
            {
                new(1, "#fff", 3, "team1"),
                new(2, "#efef", 3, "team2")
            }));
        account.AddGameConfiguration(GameConfiguration.Create("partie de sav 2", 7, 5, 500, GameMode.Flag, true,
            new Map("map2", false, new Position(2, 2)), new List<Team>()
            {
                new(3, "#fff", 3, "team1"),
                new(4, "#efef", 4, "team2")
            }));
        
        Account account2 = Account.Create(Guid.NewGuid(), "Salavatore", "Covalea", "salvo",
            "salvatore.covalea@gmail.com", "TotoSalva!1254");
        account2.AddGameConfiguration(GameConfiguration.Create("partie de Salvo", 6, 3, 600, GameMode.Flag, false,
            new Map("map de salvo", false, new Position(2, 2)), new List<Team>()
            {
                new(5, "#fff", 3, "team1"),
                new(6, "#efef", 3, "team2")
            }));
        await accountRepository.AddAsync(account, CancellationToken.None);
        await accountRepository.AddAsync(account2, CancellationToken.None);
        authenticationGateway.Authenticate(account);
    }
}