using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers
{
    public interface IDeleteHandler<T> : IRequestHandler<DeleteRequest<T>>
    { }
}
