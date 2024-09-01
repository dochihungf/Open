using System.ComponentModel;

namespace Open.Security.Enums;

public enum MFAType
{
    [Description("None")] None = 0,

    [Description("Use email")] Email = 1,

    [Description("Use phonenumber")] Phone = 2,
}
