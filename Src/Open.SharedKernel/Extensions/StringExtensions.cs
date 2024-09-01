using System.Text;
using System.Text.RegularExpressions;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static string PascalToStandard(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, "(\\B[A-Z])", " $1");
    }
    
    /// <summary>
    /// Chuyển camel case sang snake case
    /// </summary>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
    }

    /// <summary>
    /// Chuyển camel case sang snake case lower
    /// </summary>
    public static string ToSnakeCaseLower(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string
            .Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString()))
            .ToLower();
    }
    
    /// <summary>
    /// Chuyển camel case sang snake case upper
    /// </summary>
    public static string ToSnakeCaseUpper(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string
            .Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString()))
            .ToUpper();
    }

    /// <summary>
    /// Chuyển camel case sang kebab case
    /// </summary>
    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString()));
    }

    /// <summary>
    /// Chuyển camel case sang kebab case lower
    /// </summary>
    public static string ToKebabCaseLower(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string
            .Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString().ToLower() : x.ToString()))
            .ToLower();
    }
    
    /// <summary>
    /// Chuyển camel case sang kebab case lower
    /// </summary>
    public static string ToKebabCaseUpper(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string
            .Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString().ToLower() : x.ToString()))
            .ToUpper();
    }

    /// <summary>
    /// Convert string to MD5
    /// </summary>
    public static string ToMD5(this string input)
    {
        // Use input string to calculate MD5 hash
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }

    public static string ToBase64Encode(this string plainText)
    {
        if (plainText == null)
        {
            throw new ArgumentNullException();
        }

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string ToBase64Decode(this string base64EncodedData)
    {
        if (base64EncodedData == null)
        {
            throw new ArgumentNullException("");
        }

        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string ReplaceRegex(this string value, string pattern, string replacement)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.Trim();
            return Regex.Replace(value, pattern, replacement);
        }
        catch
        {
            return value;
        }
    }

    public static string NormalizeString(this string value)
    {
        try
        {
            return value.ReplaceRegex("\\s+", " ");
        }
        catch
        {
            return value;
        }
    }

    public static string ViToEn(this string unicodeString, bool special = false)
    {
        if (string.IsNullOrEmpty(unicodeString))
        {
            return string.Empty;
        }

        try
        {
            unicodeString = unicodeString.NormalizeString();
            unicodeString = unicodeString.Trim();
            unicodeString = Regex.Replace(unicodeString, "[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            unicodeString = Regex.Replace(unicodeString, "[éèẻẽẹêếềểễệ]", "e");
            unicodeString = Regex.Replace(unicodeString, "[iíìỉĩị]", "i");
            unicodeString = Regex.Replace(unicodeString, "[óòỏõọơớờởỡợôốồổỗộ]", "o");
            unicodeString = Regex.Replace(unicodeString, "[úùủũụưứừửữự]", "u");
            unicodeString = Regex.Replace(unicodeString, "[yýỳỷỹỵ]", "y");
            unicodeString = Regex.Replace(unicodeString, "[đ]", "d");
            if (special)
            {
                unicodeString = Regex.Replace(unicodeString, "[\"`~!@#$%^&*()-+=?/>.<,{}[]|]\\]", "");
            }

            unicodeString = unicodeString.Replace("\u0300", "").Replace("\u0323", "").Replace("\u0309", "")
                .Replace("\u0303", "").Replace("\u0301", "");
            return unicodeString;
        }
        catch
        {
            return "";
        }
    }

    public static bool HasUnicode(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return false;
        }

        var length = source.Length;

        source = Regex.Replace(source, "[áàảãạâấầẩẫậăắằẳẵặ]", "");
        source = Regex.Replace(source, "[éèẻẽẹêếềểễệ]", "");
        source = Regex.Replace(source, "[iíìỉĩị]", "");
        source = Regex.Replace(source, "[óòỏõọơớờởỡợôốồổỗộ]", "");
        source = Regex.Replace(source, "[úùủũụưứừửữự]", "");
        source = Regex.Replace(source, "[yýỳỷỹỵ]", "");
        source = Regex.Replace(source, "[đ]", "");

        return source.Length != length;
    }

    public static bool IsNumber(this char c)
    {
        switch (c)
        {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;

            default:
                return false;
        }
    }

    public static string StripHtml(this string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    
    public static string ToUnsignString(this string input)
    {
        input = input.Trim();
        for (int i = 0x20; i < 0x30; i++)
        {
            input = input.Replace(((char)i).ToString(), " ");
        }
        input = input.Replace(".", "-");
        input = input.Replace(" ", "-");
        input = input.Replace(",", "-");
        input = input.Replace(";", "-");
        input = input.Replace(":", "-");
        input = input.Replace("  ", "-");
        Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
        string str = input.Normalize(NormalizationForm.FormD);
        string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        while (str2.IndexOf("?") >= 0)
        {
            str2 = str2.Remove(str2.IndexOf("?"), 1);
        }
        while (str2.Contains("--"))
        {
            str2 = str2.Replace("--", "-").ToLower();
        }
        return str2.ToLower();
    }
}
