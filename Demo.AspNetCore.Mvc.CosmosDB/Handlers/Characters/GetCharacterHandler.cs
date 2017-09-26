using System.Linq;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class GetCharacterHandler : IGetSingleHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public GetCharacterHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public Character Handle(GetSingleRequest<Character> message)
        {
            return _client.Characters.GetDocumentQuery().Where(c => c.Id == message.Id).ToList().FirstOrDefault();
        }
        #endregion
    }
}
