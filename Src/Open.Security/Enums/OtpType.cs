using System.ComponentModel;

namespace Open.Security.Enums;

public enum OtpType
{
    [Description("None")] None = 0,

    [Description("For password")] Password = 1,

    [Description("For verify")] Verify = 2,

    [Description("Multi-factor Authentication")]
    MFA = 3,
}
