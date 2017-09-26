using System.Collections.Generic;
using MediatR;

namespace Demo.AspNetCore.Mvc.CosmosDB.Requests
{
    public class GetCollectionRequest<T> : IRequest<IEnumerable<T>>
    { }
}
