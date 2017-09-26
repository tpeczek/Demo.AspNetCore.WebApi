using System.Linq;
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
        public IEnumerable<Character> Handle(GetCollectionRequest<Character> message)
        {
            return _client.Characters.GetDocumentQuery().ToArray();
        }
        #endregion
    }
}
