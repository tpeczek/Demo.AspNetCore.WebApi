using System;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class GetSingleRequest<T> : IRequest<T>
    {
        #region Properties
        public string Id { get; }
        #endregion

        #region Constructor
        public GetSingleRequest(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }

            Id = id;
        }
        #endregion
    }
}
