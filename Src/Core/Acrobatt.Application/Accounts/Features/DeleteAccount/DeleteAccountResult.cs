using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.DeleteAccount;

public record DeleteAccountResult(Guid AccountId) : ICommandResult
{
}