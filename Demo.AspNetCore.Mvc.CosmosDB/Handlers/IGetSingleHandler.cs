using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers
{
    public interface IGetSingleHandler<T> : IRequestHandler<GetSingleRequest<T>, T>
    { }
}
