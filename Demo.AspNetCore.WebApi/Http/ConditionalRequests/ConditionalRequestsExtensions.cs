using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Demo.AspNetCore.WebApi.Http.ConditionalRequests
{
    internal static class ConditionalRequestsExtensions
    {
        internal static RequestConditions GetRequestConditions(this HttpRequest request)
        {
            RequestConditions requestConditions = new RequestConditions();

            RequestHeaders requestHeaders = request.GetTypedHeaders();

            if (HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method))
            {
                requestConditions.IfNoneMatch = requestHeaders.IfNoneMatch?.Select(v => v.Tag.ToString());
                requestConditions.IfModifiedSince = requestHeaders.IfModifiedSince;
            }
            else if (HttpMethods.IsPut(request.Method) || HttpMethods.IsPatch(request.Method))
            {
                requestConditions.IfMatch = requestHeaders.IfMatch?.Select(v => v.Tag.ToString());
                requestConditions.IfUnmodifiedSince = requestHeaders.IfUnmodifiedSince;
            }

            return requestConditions;
        }
    }
}
