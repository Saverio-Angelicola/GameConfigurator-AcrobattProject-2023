using Microsoft.Extensions.DependencyInjection;

namespace Acrobatt.Application.Commons.Configs.Query;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
    {
        object handler = _serviceProvider.GetRequiredService(typeof(IQueryHandler<TQuery, TResult>));

        return await ((IQueryHandler<TQuery, TResult>)handler).HandleAsync(query, cancellationToken);
    }
}