using Acrobatt.Application.Commons.Configs.Command;

namespace Acrobatt.Application.Accounts.Features.LoginAccount;

public record LoginAccount(string Email, string Password) : ICommand
{
    
}