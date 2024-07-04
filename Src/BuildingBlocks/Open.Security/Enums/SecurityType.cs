using System.ComponentModel;

namespace Open.Security.Enums;

public enum OtpType
{
    [Description("None")] None = 0,

    [Description("For password")] Password = 1,

    [Description("For verify")] Verify = 2,

    [Description("Multi-factor Authentication")] Mfa = 3,
}

public enum MfaType
{
    [Description("None")] None = 0,

    [Description("Use email")] Email = 1,

    [Description("Use phonenumber")] Phone = 2,
}

public enum AccountState
{
    [Description("Activated")] Activated = 1,

    [Description("NotActived")] NotActivated = 2,

    [Description("Blocked")] Blocked = 3,
}

public enum SecurityType
{
    Active = 0,
    InActive = 1,
}