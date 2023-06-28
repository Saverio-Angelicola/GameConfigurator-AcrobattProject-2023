using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acrobatt.Application.Accounts.Features.UpdateAccount;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.GameConfigurations.Features.GameConfigurationDetails;
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

public class GameConfigurationDetailsTests
{
    private readonly WebApplicationTestingFactory _app = new();

    [Test]
    public async Task ShowGameConfigurationDetail()
    {
        using IServiceScope scope = _app.Services.CreateScope();
        IQueryDispatcher queryDispatcher =
            (IQueryDispatcher)scope.ServiceProvider.GetRequiredService(typeof(IQueryDispatcher));
        IAccountRepository accountRepository =
            (IAccountRepository)scope.ServiceProvider.GetRequiredService(typeof(IAccountRepository));
        IAuthenticationGateway authenticationGateway =
            (IAuthenticationGateway)scope.ServiceProvider.GetRequiredService(typeof(IAuthenticationGateway));
        Account account = Account.Create(Guid.NewGuid(), "saverio", "angelicola", "sangelicola",
            "angelicola.saverio@gmail.com", "Toto!1234");
        account.AddGameConfiguration(GameConfiguration.Create("partie1", 2, 3, 600, GameMode.Flag, false,
            new Map("map1", false, new Position(2, 2)), new List<Team>()
            {
                new(1, "#fff", 3, "team1"),
                new(2, "#efef", 3, "team2")
            }));
        await accountRepository.AddAsync(account, CancellationToken.None);
        authenticationGateway.Authenticate(account);
        GameConfigurationDetail command = new(1);
        GameConfigurationDetailViewModel result = await queryDispatcher.DispatchAsync<GameConfigurationDetail, GameConfigurationDetailViewModel>(command,
                CancellationToken.None);
        result.Id.Should().Be(1);
        result.GameName.Should().Be("partie1");
    }
}