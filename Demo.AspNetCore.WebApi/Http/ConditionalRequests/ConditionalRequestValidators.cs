using System;

namespace Demo.AspNetCore.WebApi.Http.ConditionalRequests
{
    internal readonly struct ConditionalRequestValidators
    {
        public string EntityTag { get; }

        public DateTime? LastModified { get; }

        public ConditionalRequestValidators(string entityTag, DateTime? lastModified)
            : this()
        {
            EntityTag = entityTag;
            LastModified = lastModified;
        }
    }
}
