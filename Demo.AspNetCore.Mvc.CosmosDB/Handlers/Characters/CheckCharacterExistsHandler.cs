using System;
using System.Linq;
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
        public bool Handle(CheckExistsRequest<Character> message)
        {
            return (!String.IsNullOrWhiteSpace(message.OtherThanId) ?
               _client.Characters.GetDocumentQuery().Where(c => (c.Id != message.OtherThanId) && (c.Name == message.Name))
               : _client.Characters.GetDocumentQuery().Where(c => c.Name == message.Name)).Take(1).ToArray().Length > 0;
        }
        #endregion
    }
}
