using Acrobatt.Application.Commons.Contracts.Providers;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace Acrobatt.Infrastructure.Providers;

public class StorageProvider : IStorageProvider
{
    private readonly IConfiguration _configuration;
    private readonly string _awsAccessKey;
    private readonly string _awsSecretKey;
    private readonly AmazonS3Config _awsConfig;
    private readonly string _bucketName;

    public StorageProvider(IConfiguration configuration)
    {
        _configuration = configuration;
        _awsAccessKey = configuration.GetSection("AWS:accessKey").Value;
        _awsSecretKey = configuration.GetSection("AWS:secretKey").Value;
        _bucketName = _configuration.GetSection("AWS:bucketName").Value;
        _awsConfig =  new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUWest3
        };
    }

    public async Task<Stream> GetFileAsync(string filename, CancellationToken cancellationToken)
    {
        BasicAWSCredentials credentials = new(_awsAccessKey, _awsSecretKey);
        TransferUtilityOpenStreamRequest downloadRequest = new()
        {
            BucketName = _bucketName,
            Key = filename
        };
        
        using AmazonS3Client client = new(credentials, _awsConfig);
        TransferUtility transferUtility = new(client);

        return await transferUtility.OpenStreamAsync(downloadRequest, cancellationToken);
    }

    public async Task UploadFileAsync(string filename, MemoryStream fileStream, CancellationToken cancellationToken)
    {
        BasicAWSCredentials credentials = new(_awsAccessKey, _awsSecretKey);

        TransferUtilityUploadRequest uploadRequest = new()
        {
            InputStream = fileStream,
            Key = filename,
            BucketName = _bucketName,
            CannedACL = S3CannedACL.NoACL
        };
        
        using AmazonS3Client client = new(credentials, _awsConfig);
        TransferUtility transferUtility = new(client);
        
        await transferUtility.UploadAsync(uploadRequest, cancellationToken);
        
        fileStream.Close();
    }
}