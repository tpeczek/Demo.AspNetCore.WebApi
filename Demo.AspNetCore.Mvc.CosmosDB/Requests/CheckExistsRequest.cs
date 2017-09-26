using System;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class CheckExistsRequest<T> : IRequest<bool>
    {
        #region Properties
        public string Name { get; }

        public string OtherThanId { get; }
        #endregion

        #region Constructors
        public CheckExistsRequest(string name)
            : this(name, null)
        { }

        public CheckExistsRequest(string name, string otherThanId)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;
            OtherThanId = otherThanId;
        }
        #endregion
    }
}
