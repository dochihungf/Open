using System.Text.RegularExpressions;

namespace Open.SharedKernel.Libraries.Helpers;

public static class StringHelper
{
    public static string RemoveExtraWhitespace(string? statement)
    {
        if (statement == null)
        {
            return "";
        }

        statement = statement.Trim();

        var currentLength = statement.Length;
        while (true)
        {
            statement = statement.Replace("  ", " ");
            if (currentLength == statement.Length)
            {
                return statement;
            }
            currentLength = statement.Length;
        }
    }

    public static string RemoveSpecialCharacters(string input)
    {
        // This code will remove all of the special characters
        // but if you doesn't want to remove some of the special character for e.g. comma "," and colon ":"
        // then make changes like this: Regex.Replace(Your String, @"[^0-9a-zA-Z:,]+", "")  
        return Regex.Replace(input, @"[^0-9a-zA-Z]+", "");
    }
}
