namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static bool IsPrimitive(this Type type)
    {
        switch (type.Name)
        {
            case "Boolean":
            case "Byte":
            case "SByte":
            case "Int16":
            case "Int32":
            case "Int64":
            case "UInt16":
            case "UInt32":
            case "UInt64":
            case "Char":
            case "Double":
            case "Single":
                return true;

            default:
                return false;
        }
    }
}
