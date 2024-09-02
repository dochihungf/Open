using System.ComponentModel;

namespace Open.Identity.Domain.Enums;

public enum MFAType
{
    [Description("None")]
    None = 0,

    [Description("Use email")]
    Email = 1,

    [Description("Use phonenumber")]
    Phone = 2,
}
