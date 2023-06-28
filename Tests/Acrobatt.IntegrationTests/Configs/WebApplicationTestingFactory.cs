using System;
using System.Linq;
using Acrobatt.API;
using Acrobatt.Application.Commons.Configs.Command;
using Acrobatt.Application.Commons.Configs.Query;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.Gateways;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Acrobatt.Infrastructure.Persistence.Repositories.Read;
using Acrobatt.Infrastructure.Persistence.Repositories.write;
using Acrobatt.Infrastructure.Providers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Acrobatt.IntegrationTests.Configs;

public class WebApplicationTestingFactory : WebApplicationFactory<Program>
{
    // Gives a fixture an opportunity to configure the application before it gets built.
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("acrobattTest")
            .WithUsername("acrobattTest")
            .WithPassword("test")
            .Build();

        postgreSqlContainer.StartAsync().Wait();
        builder.ConfigureTestServices(services =>
        {
            // Remove AppDbContext
            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PostgresContext>));
            if (descriptor != null) services.Remove(descriptor);
            
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // register automatically command handler in IOC Container
            services.Scan(s => s.FromCallingAssembly()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        
            // register automatically query handler in IOC Container
            services.Scan(s => s.FromCallingAssembly()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            // Add DB context pointing to test container
            services.AddHttpContextAccessor();
            services.AddDbContext<PostgresContext>(options => options.UseNpgsql(postgreSqlContainer.GetConnectionString()));
        
            services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
            services.AddScoped<IGuidProvider, GuidProvider>();
            services.AddScoped<IPasswordProvider, PasswordProvider>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthenticationGateway, AuthenticationGateway>(AuthenticationGateway.AddGateway);

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountReadRepository, AccountReadRepository>();
            services.AddScoped<IGameConfigurationReadRepository, GameConfigurationReadRepository>();
            services.AddScoped<IStorageProvider, StorageProvider>();
            services.AddScoped<IGameGeneratorProvider, GameGeneratorProvider>();

            // Ensure schema gets created
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            PostgresContext context = scopedServices.GetRequiredService<PostgresContext>();
            context.Database.EnsureCreated();
        });
    }
}