using System;
using System.Linq;
using System.Threading;
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
        public async Task<Character> Handle(UpdateRequest<Character> request, CancellationToken cancellationToken)
        {
            CharacterDocument characterDocument = await _client.Characters.GetDocumentQuery().Where(c => c.Id == request.Id).Take(1).ToAsyncEnumerable().FirstOrDefault();
            
            if (characterDocument != null)
            {
                CheckPreconditions(request, characterDocument);

                characterDocument.Name = request.Update.Name;
                characterDocument.Gender = request.Update.Gender;
                characterDocument.Height = request.Update.Height;
                characterDocument.Weight = request.Update.Weight;
                characterDocument.BirthYear = request.Update.BirthYear;
                characterDocument.SkinColor = request.Update.SkinColor;
                characterDocument.HairColor = request.Update.HairColor;
                characterDocument.EyeColor = request.Update.EyeColor;
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
