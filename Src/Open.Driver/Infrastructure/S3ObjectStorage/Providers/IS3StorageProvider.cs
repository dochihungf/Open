using Open.Driver.Infrastructure.S3ObjectStorage.Models;

namespace Open.Driver.Infrastructure.S3ObjectStorage.Providers;

public interface IS3StorageProvider : IStorageProvider
{
    /// <summary>
    /// Trả về tất cả file trong directory
    /// </summary>
    Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default);

    /// <summary>
    /// Trả về file paging trong directory
    /// </summary>
    Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "", CancellationToken cancellationToken = default);
}
