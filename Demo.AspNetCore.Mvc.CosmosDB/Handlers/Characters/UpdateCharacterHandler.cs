using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class UpdateCharacterHandler : IUpdateHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public UpdateCharacterHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(UpdateRequest<Character> message)
        {
            CharacterDocument characterDocument = _client.Characters.GetDocumentQuery().Where(c => c.Id == message.Id).Take(1).ToList().FirstOrDefault();
            
            if (characterDocument != null)
            {
                characterDocument.Name = message.Update.Name;
                characterDocument.Gender = message.Update.Gender;
                characterDocument.Height = message.Update.Height;
                characterDocument.Weight = message.Update.Weight;
                characterDocument.BirthYear = message.Update.BirthYear;
                characterDocument.SkinColor = message.Update.SkinColor;
                characterDocument.HairColor = message.Update.HairColor;
                characterDocument.EyeColor = message.Update.EyeColor;

                await _client.Characters.ReplaceDocumentAsync(characterDocument);
            }

            return characterDocument;
        }
        #endregion
    }
}
