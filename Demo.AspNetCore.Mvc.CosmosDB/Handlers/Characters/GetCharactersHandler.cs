using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class GetCharactersHandler : IGetCollectionHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public GetCharactersHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<Character>> Handle(GetCollectionRequest<Character> request, CancellationToken cancellationToken)
        {
            return await _client.Characters.GetDocumentQuery().ToAsyncEnumerable().ToArray();
        }
        #endregion
    }
}
