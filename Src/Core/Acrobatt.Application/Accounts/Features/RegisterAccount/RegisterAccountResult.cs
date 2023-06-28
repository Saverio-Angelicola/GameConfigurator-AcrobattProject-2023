using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.RegisterAccount;

public record RegisterAccountResult(Guid AccountId) : ICommandResult
{
}