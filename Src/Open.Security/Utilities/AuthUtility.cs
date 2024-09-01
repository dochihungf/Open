using System.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Open.Security.Constants;
using Open.Security.Models;

namespace Open.Security.Utilities;

public static class AuthUtility
{
    /// <summary>
    /// Kiểm tra endpoint có cần authorize không?
    /// </summary>
    public static bool EndpointRequiresAuthorize(ResourceExecutingContext context)
    {
        var endpointMetadata = context.ActionDescriptor.EndpointMetadata;
        var allowAnonymous = endpointMetadata.FirstOrDefault(x =>
            x.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute));
        if (allowAnonymous != null)
        {
            return false;
        }

        var authorize = endpointMetadata.FirstOrDefault(x =>
            x.GetType() == typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));
        if (authorize == null)
        {
            return false;
        }

        return true;
    }

    public static string? GetCurrentRequestId(HttpContext context)
    {
        return context.Request.Headers[HeaderNames.RequestId].First();
    }

    public static string FromExponentToPermission(int exponent)
    {
        var result = new BigInteger(1);
        var two = new BigInteger(2);

        for (int i = 1; i <= exponent; i++)
            result *= two;

        return result.ToString();
    }

    public static string CalculateToTalPermission(IEnumerable<int> exponents)
    {
        var result = new BigInteger(0);
        foreach (var exponent in exponents)
            result += BigInteger.Parse(FromExponentToPermission(exponent));
        return result.ToString();
    }

    public static string ConvertToBinary(string input)
    {
        var result = "";
        var parse = BigInteger.Parse(input);
        var two = new BigInteger(2);

        while (true)
        {
            var b = parse % two;
            parse = parse / two;
            result += b;
            if (parse.IsZero)
            {
                break;
            }
        }

        return string.Join("", result.Reverse());
    }

    public static bool ComparePermissionAsString(string permission, string actionPermission)
    {
        if (string.IsNullOrEmpty(permission) || string.IsNullOrEmpty(actionPermission))
        {
            return false;
        }

        var left = ConvertToBinary(permission);
        var right = ConvertToBinary(actionPermission);
        var offset = "";
        var andResult = "";

        // Ensure Left always greater than Right
        if (right.Length > left.Length)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }

        // Compensate for the number on the right
        for (int i = 1; i <= left.Length - right.Length; i++)
        {
            offset += "0";
        }

        right = offset + right;

        // Execute bitwise &
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] == right[i] && left[i] == '1')
                andResult += "1";
            else
                andResult += "0";
        }

        return andResult.Equals(right);
    }

    public static string TryGetIp(HttpRequest request)
    {
        var ip = request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrEmpty(ip))
        {
            return ip;
        }

        var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
        if (remoteIpAddress != null)
        {
            // If we got an IPV6 address, then we need to ask the network for the IPV4 address
            // This usually only happens when the browser is on the same machine as the server.
            if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            ip = remoteIpAddress.ToString();
        }

        return ip;
    }
    
}
