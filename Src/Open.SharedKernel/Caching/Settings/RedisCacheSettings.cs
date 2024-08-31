namespace Open.SharedKernel.Caching.Settings;

public class RedisCacheSettings
{
    public const string SectionName = "DistributedCache";
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
    public int DatabaseIndex { get; set; } = 0;
}
