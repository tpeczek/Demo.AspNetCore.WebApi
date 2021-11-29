using System;
using System.Linq;
using System.Collections.Generic;

namespace Demo.AspNetCore.WebApi.Http.ConditionalRequests
{
    internal class RequestConditions
    {
        #region Properties
        public IEnumerable<string> IfNoneMatch { get; set; }

        public DateTimeOffset? IfModifiedSince { get; set; }

        public IEnumerable<string> IfMatch { get; set; }

        public DateTimeOffset? IfUnmodifiedSince { get; set; }
        #endregion

        #region Methods
        public bool CheckPreconditionFailed(ConditionalRequestValidators validators)
        {
            if (IfMatch != null && IfMatch.Any())
            {
                if ((IfMatch.Count() > 2) || (IfMatch.First() != "*"))
                {
                    if (!IfMatch.Contains(validators.EntityTag))
                    {
                        return true;
                    }
                }
            }
            else if (IfUnmodifiedSince.HasValue)
            {
                DateTimeOffset lastModified = validators.LastModified.Value.AddTicks(-(validators.LastModified.Value.Ticks % TimeSpan.TicksPerSecond));

                if (lastModified > IfUnmodifiedSince.Value)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
