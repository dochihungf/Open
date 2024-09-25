using Open.Driver.Infrastructure.S3ObjectStorage.Models;

namespace Open.Driver.Infrastructure.S3ObjectStorage;

public interface IStorageProvider
{
    /// <summary>
    /// Upload file
    /// </summary>
    Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Upload multi files
    /// </summary>
    Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default);

    /// <summary>
    /// Download file
    /// </summary>
    Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Download file
    /// </summary>
    Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove file
    /// </summary>
    Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default);
}
