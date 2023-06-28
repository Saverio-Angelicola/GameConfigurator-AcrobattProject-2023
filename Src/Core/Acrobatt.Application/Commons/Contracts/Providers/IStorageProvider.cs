namespace Acrobatt.Application.Commons.Contracts.Providers;

public interface IStorageProvider
{
    /// <summary>
    /// Get file from AWS S3 bucket by filename
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> GetFileAsync(string filename, CancellationToken cancellationToken);
    
    /// <summary>
    ///  Upload a new file in AWS S3 bucket
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="fileStream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UploadFileAsync(string filename, MemoryStream fileStream, CancellationToken cancellationToken);
}