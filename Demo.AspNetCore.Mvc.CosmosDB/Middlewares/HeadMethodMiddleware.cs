using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Demo.AspNetCore.Mvc.CosmosDB.Middlewares
{
    public class HeadMethodMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        #endregion

        #region Constructor
        public HeadMethodMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            bool methodSwitched = false;

            if (HttpMethods.IsHead(context.Request.Method))
            {
                methodSwitched = true;

                context.Request.Method = HttpMethods.Get;
                context.Response.Body = Stream.Null;
            }

            await _next(context);

            if (methodSwitched)
            {
                context.Request.Method = HttpMethods.Head;
            }
        }
        #endregion
    }
}
