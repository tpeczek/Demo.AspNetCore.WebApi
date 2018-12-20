using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class CheckCharacterExistsHandler : ICheckExistsHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public CheckCharacterExistsHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public async Task<bool> Handle(CheckExistsRequest<Character> request, CancellationToken cancellationToken)
        {
            return await (!String.IsNullOrWhiteSpace(request.OtherThanId) ?
               _client.Characters.GetDocumentQuery().Where(c => (c.Id != request.OtherThanId) && (c.Name == request.Name))
               : _client.Characters.GetDocumentQuery().Where(c => c.Name == request.Name)).Take(1).ToAsyncEnumerable().Any();
        }
        #endregion
    }
}
