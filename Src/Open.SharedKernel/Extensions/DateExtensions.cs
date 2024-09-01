using System.Globalization;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static string DateFullText(this DateTime date)
    {
        var ampm = date.ToString("tt", CultureInfo.InvariantCulture);
        var time = date.ToString("dd/MM/yyyy hh:mm:ss").Split(" ");
        return $"{time[0]} {time[1]}{ampm}";
    }

    public static int DiffDays(this DateTime date, DateTime date2)
    {
        var diffDays = Math.Ceiling(Math.Abs((date - date2).TotalDays));
        return Convert.ToInt32(diffDays);
    }
}
