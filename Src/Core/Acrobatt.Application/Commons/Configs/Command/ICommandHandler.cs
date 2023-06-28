namespace Acrobatt.Application.Commons.Configs.Command;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand where TResult : ICommandResult
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}