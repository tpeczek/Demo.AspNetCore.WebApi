using System;
using System.Collections.Generic;

namespace Demo.AspNetCore.Mvc.CosmosDB.Http
{
    internal class HttpRequestConditions
    {
        #region Properties
        public IEnumerable<string> IfNoneMatch { get; set; }

        public DateTimeOffset? IfModifiedSince { get; set; }

        public IEnumerable<string> IfMatch { get; set; }

        public DateTimeOffset? IfUnmodifiedSince { get; set; }
        #endregion
    }
}
