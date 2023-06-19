using System.Text;
using Acrobatt.Application.Commons.Contracts.Providers;
using Acrobatt.Application.Commons.Contracts.ReadRepositories;
using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Gateways;
using Acrobatt.Infrastructure.Gateways;
using Acrobatt.Infrastructure.Persistence.Contexts;
using Acrobatt.Infrastructure.Persistence.Repositories.Read;
using Acrobatt.Infrastructure.Persistence.Repositories.write;
using Acrobatt.Infrastructure.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Acrobatt.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("default");
        
        services.AddHttpContextAccessor();
        services.AddDbContext<PostgresContext>(options => options.UseNpgsql(connectionString));
        
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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:SecretToken").Value ?? throw new InvalidOperationException())),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
            };
        });

        return services;
    }
}