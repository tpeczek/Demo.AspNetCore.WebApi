using System;
using System.Collections.Generic;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class UpdateRequest<T> : IRequest<T>
    {
        #region Properties
        public string Id { get; }

        public T Update { get; }

        public IEnumerable<string> IfMatch { get; }

        public DateTimeOffset? IfUnmodifiedSince { get; }
        #endregion

        #region Constructor
        public UpdateRequest(string id, T update)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            Id = id;
            Update = update;
        }

        public UpdateRequest(string id, T update, IEnumerable<string> ifMatch, DateTimeOffset? ifUnmodifiedSince)
            : this(id, update)
        {
            IfMatch = ifMatch;
            IfUnmodifiedSince = ifUnmodifiedSince;
        }
        #endregion
    }
}
