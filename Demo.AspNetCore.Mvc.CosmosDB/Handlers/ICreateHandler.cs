using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers
{
    public interface ICreateHandler<T> : IRequestHandler<CreateRequest<T>, T>
    { }
}
