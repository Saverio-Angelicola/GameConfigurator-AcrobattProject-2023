using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.UpdateAccount;

public record UpdateAccount(string FirstName, string LastName, string Pseudo, string Email) : ICommand
{
}