using System.Collections.Generic;
using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers
{
    public interface IGetCollectionHandler<T> : IRequestHandler<GetCollectionRequest<T>, IEnumerable<T>>
    { }
}
