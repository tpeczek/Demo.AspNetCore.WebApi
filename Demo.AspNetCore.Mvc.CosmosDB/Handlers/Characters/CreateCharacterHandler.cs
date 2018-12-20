using System;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;
using System.Threading;

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
        public async Task<Character> Handle(CreateRequest<Character> request, CancellationToken cancellationToken)
        {
            CharacterDocument characterDocument = new CharacterDocument
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = request.Item.Name,
                Gender = request.Item.Gender,
                Height = request.Item.Height,
                Weight = request.Item.Weight,
                BirthYear = request.Item.BirthYear,
                SkinColor = request.Item.SkinColor,
                HairColor = request.Item.HairColor,
                EyeColor = request.Item.EyeColor,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            await _client.Characters.CreateDocumentAsync(characterDocument);

            return characterDocument;
        }
        #endregion
    }
}
