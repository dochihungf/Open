using Amazon.S3;
using Microsoft.Extensions.Options;
using Open.Driver.Infrastructure.S3ObjectStorage.Models;

namespace Open.Driver.Infrastructure.S3ObjectStorage.Providers;

public class S3StorageProvider : IS3StorageProvider, IDisposable
{
    private readonly ILogger<S3StorageProvider> _logger;
    private readonly AmazonS3Client _client;
    private readonly IOptions<S3StorageSettings> _options;
    private const int ExpiryTime = 365; // days;

    public S3StorageProvider(
        ILogger<S3StorageProvider> logger, 
        AmazonS3Client client, 
        IOptions<S3StorageSettings> options)
    {
        _logger = logger;
        _client = client;
        _options = options;
    }

    public Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "",
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
