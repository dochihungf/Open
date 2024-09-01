using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;

namespace Open.SharedKernel.Extensions;

public static partial class Extensions
{
    public static HttpContext Clone(this HttpContext httpContext, bool copyBody = true)
        {
            var existingRequestFeature = httpContext.Features.Get<IHttpRequestFeature>();
            var requestHeaders = new Dictionary<string, StringValues>(existingRequestFeature.Headers.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var header in existingRequestFeature.Headers)
            {
                requestHeaders[header.Key] = header.Value;
            }

            var requestFeature = new HttpRequestFeature
            {
                Protocol = existingRequestFeature.Protocol,
                Method = existingRequestFeature.Method,
                Scheme = existingRequestFeature.Scheme,
                Path = existingRequestFeature.Path,
                PathBase = existingRequestFeature.PathBase,
                QueryString = existingRequestFeature.QueryString,
                RawTarget = existingRequestFeature.RawTarget,
                Headers = new HeaderDictionary(requestHeaders),
            };

            if (copyBody)
            {
                // We need to buffer first, otherwise the body won't be copied
                // Won't work if the body stream was accessed already without calling EnableBuffering() first or without leaveOpen
                httpContext.Request.EnableBuffering();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                requestFeature.Body = existingRequestFeature.Body;
            }

            var features = new FeatureCollection();
            features.Set<IHttpRequestFeature>(requestFeature);
            // Unless we need the response we can ignore it...
            //features.Set<IHttpResponseFeature>(new HttpResponseFeature());
            //features.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(Stream.Null));

            var newContext = new DefaultHttpContext(features);
            if (copyBody)
            {
                // Rewind for any future use...
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            // Can happen if the body was not copied
            if (httpContext.Request.HasFormContentType && httpContext.Request.Form.Count != newContext.Request.Form.Count)
            {
                newContext.Request.Form = new FormCollection(httpContext.Request.Form.ToDictionary(f => f.Key, f => f.Value));
            }

            return newContext;
        }
}
