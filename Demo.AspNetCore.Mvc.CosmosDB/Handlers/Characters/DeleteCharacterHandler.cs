using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class DeleteCharacterHandler : IDeleteHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public DeleteCharacterHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public Task<Unit> Handle(DeleteRequest<Character> request, CancellationToken cancellationToken)
        {
            return (Task<Unit>)_client.Characters.DeleteDocumentAsync(request.Item.Id);
        }
        #endregion
    }
}
