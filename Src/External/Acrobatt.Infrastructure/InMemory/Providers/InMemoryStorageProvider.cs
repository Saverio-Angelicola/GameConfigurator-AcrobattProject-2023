using Acrobatt.Application.Commons.Contracts.Providers;

namespace Acrobatt.Infrastructure.InMemory.Providers;

public sealed class InMemoryStorageProvider : IStorageProvider
{
    public async Task<Stream> GetFileAsync(string filename, CancellationToken cancellationToken)
    {
        return Stream.Null;
    }

    public Task UploadFileAsync(string filename, MemoryStream fileStream, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}