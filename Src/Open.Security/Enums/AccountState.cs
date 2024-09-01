using System.ComponentModel;

namespace Open.Security.Enums;

public enum AccountState
{
    [Description("Actived")] Actived = 1,

    [Description("NotActived")] NotActived = 2,

    [Description("Blocked")] Blocked = 3,
}
