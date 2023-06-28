namespace Acrobatt.Application.Commons.Configs.Query;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery
{
    public Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}