using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.RegisterAccount;

public record RegisterAccount(string FirstName, string LastName, string Pseudo, string Email, string Password) : ICommand
{
}