namespace Acrobatt.Application.Commons.Contracts.Providers;

public interface IJwtProvider
{
    string CreateJwt(Guid accountId);
}