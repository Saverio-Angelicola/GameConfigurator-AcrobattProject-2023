namespace Acrobatt.Application.Commons.Configs.Command;

public interface ICommandDispatcher
{
    Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand where TResult : ICommandResult;
}