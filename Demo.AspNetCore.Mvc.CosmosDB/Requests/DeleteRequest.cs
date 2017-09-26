using System;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class DeleteRequest<T> : IRequest
    {
        #region Properties
        public T Item { get; }
        #endregion

        #region Constructor
        public DeleteRequest(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item)); ;
            }

            Item = item;
        }
        #endregion
    }
}
