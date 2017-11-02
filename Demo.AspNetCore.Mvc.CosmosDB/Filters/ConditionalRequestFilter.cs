using System;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Http;

namespace Demo.AspNetCore.Mvc.CosmosDB.Filters
{
    internal class ConditionalRequestFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            IConditionalRequestMetadata metadata = (context.Result as ObjectResult)?.Value as IConditionalRequestMetadata;
            if (metadata != null)
            {
                if (CheckModified(context, metadata))
                {
                    SetConditionalMetadataHeaders(context, metadata);
                }
            }
        }

        private static bool CheckModified(ResultExecutingContext context, IConditionalRequestMetadata metadata)
        {
            bool modified = true;

            HttpRequestConditions requestConditions = context.HttpContext.Request.GetRequestConditions();

            if ((requestConditions.IfNoneMatch != null) && requestConditions.IfNoneMatch.Any())
            {
                if (!String.IsNullOrWhiteSpace(metadata.EntityTag) && requestConditions.IfNoneMatch.Contains(metadata.EntityTag))
                {
                    modified = false;
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }
            else if (requestConditions.IfModifiedSince.HasValue && metadata.LastModified.HasValue)
            {
                DateTimeOffset lastModified = metadata.LastModified.Value.AddTicks(-(metadata.LastModified.Value.Ticks % TimeSpan.TicksPerSecond));

                if (lastModified <= requestConditions.IfModifiedSince.Value)
                {
                    modified = false;
                    context.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }

            return modified;
        }

        private static void SetConditionalMetadataHeaders(ResultExecutingContext context, IConditionalRequestMetadata metadata)
        {
            ResponseHeaders responseHeaders = context.HttpContext.Response.GetTypedHeaders();

            if (!String.IsNullOrWhiteSpace(metadata.EntityTag))
            {
                responseHeaders.ETag = new EntityTagHeaderValue(metadata.EntityTag, true);
            }

            if (metadata.LastModified.HasValue)
            {
                responseHeaders.LastModified = metadata.LastModified.Value;
            }
        }
    }
}
