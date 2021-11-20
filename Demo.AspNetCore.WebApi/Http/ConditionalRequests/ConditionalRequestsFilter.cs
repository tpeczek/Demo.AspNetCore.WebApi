using System;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.AspNetCore.WebApi.Http.ConditionalRequests
{
    internal class ConditionalRequestsFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            IConditionalRequestMetadata metadata = (context.Result as ObjectResult)?.Value as IConditionalRequestMetadata;
            if (metadata != null)
            {
                ConditionalRequestValidators validators = metadata.GetValidators();
                if (!CheckNotModified(context, validators))
                {
                    SetConditionalMetadataHeaders(context, validators);
                }
            }
        }

        private static bool CheckNotModified(ResultExecutingContext context, ConditionalRequestValidators validators)
        {
            bool notModified = false;

            RequestConditions requestConditions = context.HttpContext.Request.GetRequestConditions();

            if ((requestConditions.IfNoneMatch != null) && requestConditions.IfNoneMatch.Any())
            {
                if (!String.IsNullOrWhiteSpace(validators.EntityTag) && requestConditions.IfNoneMatch.Contains(validators.EntityTag))
                {
                    notModified = true;
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }
            else if (requestConditions.IfModifiedSince.HasValue && validators.LastModified.HasValue)
            {
                DateTimeOffset lastModified = validators.LastModified.Value.AddTicks(-(validators.LastModified.Value.Ticks % TimeSpan.TicksPerSecond));

                if (lastModified <= requestConditions.IfModifiedSince.Value)
                {
                    notModified = true;
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }

            return notModified;
        }

        private static void SetConditionalMetadataHeaders(ResultExecutingContext context, ConditionalRequestValidators validators)
        {
            ResponseHeaders responseHeaders = context.HttpContext.Response.GetTypedHeaders();

            if (!String.IsNullOrWhiteSpace(validators.EntityTag))
            {
                responseHeaders.ETag = new EntityTagHeaderValue(validators.EntityTag, true);
            }

            if (validators.LastModified.HasValue)
            {
                responseHeaders.LastModified = validators.LastModified.Value;
            }
        }
    }
}
