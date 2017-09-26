using System;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class CreateCharacterHandler : ICreateHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public CreateCharacterHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(CreateRequest<Character> message)
        {
            CharacterDocument characterDocument = new CharacterDocument
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = message.Item.Name,
                Gender = message.Item.Gender,
                Height = message.Item.Height,
                Weight = message.Item.Weight,
                BirthYear = message.Item.BirthYear,
                SkinColor = message.Item.SkinColor,
                HairColor = message.Item.HairColor,
                EyeColor = message.Item.EyeColor,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            await _client.Characters.CreateDocumentAsync(characterDocument);

            return characterDocument;
        }
        #endregion
    }
}
