using Microsoft.Extensions.DependencyInjection;

namespace Acrobatt.Application.Commons.Configs.Command;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand where TResult : ICommandResult
    {
        object handler = _serviceProvider.GetRequiredService(typeof(ICommandHandler<TCommand, TResult>));

        return await ((ICommandHandler<TCommand, TResult>)handler).HandleAsync(command, cancellationToken);
    }
}