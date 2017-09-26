using System;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class CreateRequest<T> : IRequest<T>
    {
        #region Properties
        public T Item { get; }
        #endregion

        #region Constructor
        public CreateRequest(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Item = item;
        }
        #endregion
    }
}
