using Microsoft.Extensions.Configuration;

namespace Open.SharedKernel.Settings;

public static class CoreSettings
{
    public static readonly bool IsSingleDevice = false;
    
    public static Dictionary<string, string>? ConnectionStrings { get; private set; }
    
    public static List<string> Black3pKeywords { get; private set; }

    public static void SetConnectionStrings(IConfiguration configuration)
    {
        ConnectionStrings = configuration.GetRequiredSection("ConnectionStrings").Get<Dictionary<string, string>>();
    }

}
