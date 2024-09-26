using System.ComponentModel.DataAnnotations;

namespace Open.Driver.Infrastructure.S3ObjectStorage;

public sealed class S3StorageSettings
{
    public string AccessKey { get; private set; }
    public string SecretKey { get; private set; }
    public string AccountId { get; private set; }
    public string ServiceURL { get; set; }
    public string BucketName { get; private set; }
    public string Root { get; private set; }
    public List<string> AcceptExtensions { get; private set; }
    
    [Required, Range(3, 7)]
    public int MaxErrorRetry { get; set; }
}
