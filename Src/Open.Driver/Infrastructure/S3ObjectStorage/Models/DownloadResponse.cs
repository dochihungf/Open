namespace Open.Driver.Infrastructure.S3ObjectStorage.Models;

public class DownloadResponse
{
    public string PresignedUrl { get; set; }

    public int ExpiryTime { get; set; }

    public string FileName { get; set; }

    public MemoryStream MemoryStream { get; set; }
}
