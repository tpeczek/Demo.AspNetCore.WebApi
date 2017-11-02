using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;
using Demo.AspNetCore.Mvc.CosmosDB.Exceptions;

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
                CheckPreconditions(message, characterDocument);

                characterDocument.Name = message.Update.Name;
                characterDocument.Gender = message.Update.Gender;
                characterDocument.Height = message.Update.Height;
                characterDocument.Weight = message.Update.Weight;
                characterDocument.BirthYear = message.Update.BirthYear;
                characterDocument.SkinColor = message.Update.SkinColor;
                characterDocument.HairColor = message.Update.HairColor;
                characterDocument.EyeColor = message.Update.EyeColor;
                characterDocument.LastUpdatedDate = DateTime.UtcNow;

                await _client.Characters.ReplaceDocumentAsync(characterDocument);
            }

            return characterDocument;
        }

        private void CheckPreconditions<T>(UpdateRequest<T> message, T update) where T: IConditionalRequestMetadata
        {
            if ((message.IfMatch) != null && message.IfMatch.Any())
            {
                if ((message.IfMatch.Count() > 2) || (message.IfMatch.First() != "*"))
                {
                    if (!message.IfMatch.Contains(update.EntityTag))
                    {
                        throw new PreconditionFailedException();
                    }
                }
            }
            else if (message.IfUnmodifiedSince.HasValue)
            {
                DateTimeOffset lastModified = update.LastModified.Value.AddTicks(-(update.LastModified.Value.Ticks % TimeSpan.TicksPerSecond));

                if (lastModified > message.IfUnmodifiedSince.Value)
                {
                    throw new PreconditionFailedException();
                }
            }
        }
        #endregion
    }
}
