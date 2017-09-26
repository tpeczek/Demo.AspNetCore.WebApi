using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers
{
    public interface ICheckExistsHandler<T> : IRequestHandler<CheckExistsRequest<T>, bool>
    { }
}
