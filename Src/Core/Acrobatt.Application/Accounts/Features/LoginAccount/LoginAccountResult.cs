using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.LoginAccount;

public record LoginAccountResult(string Token, Guid Id, string Email, string FirstName, string LastName, string Pseudo) : ICommandResult
{
}