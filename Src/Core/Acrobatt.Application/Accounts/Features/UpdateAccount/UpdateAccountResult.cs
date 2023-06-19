using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.UpdateAccount;

public record UpdateAccountResult(string FirstName, string LastName, string Pseudo, string Email) : ICommandResult
{
    
}