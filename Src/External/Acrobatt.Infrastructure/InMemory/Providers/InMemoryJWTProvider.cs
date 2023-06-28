using Acrobatt.Application.Commons.Contracts.Providers;

namespace Acrobatt.Infrastructure.InMemory.Providers;

public sealed class InMemoryJWTProvider : IJwtProvider
{
    public string CreateJwt(Guid accountId)
    {
        return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlNhbmdlbGljb2xhIiwiaWF0Ijo1NTE2MjM5MDIyfQ.s94ZnaHZ2e24EsX_p_LHDUzhoz5wdu1yr7TGFkmwgGo";
    }
}