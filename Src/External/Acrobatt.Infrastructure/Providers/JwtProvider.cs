using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Acrobatt.Application.Commons.Contracts.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Acrobatt.Infrastructure.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateJwt(Guid accountId)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, accountId.ToString())
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:SecretToken").Value ?? throw new InvalidOperationException()));

        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken token = new(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}