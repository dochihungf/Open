using System.ComponentModel;

namespace Open.Identity.Domain.Enums;

public enum OTPType
{
    [Description("None")]
    None = 0,

    [Description("For password")]
    Password = 1,

    [Description("For verify")]
    Verify = 2,

    [Description("Multi-factor Authentication")]
    MFA = 3,
}
