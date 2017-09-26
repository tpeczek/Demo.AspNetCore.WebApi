using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;
using Demo.AspNetCore.Mvc.CosmosDB.Model;

namespace Demo.AspNetCore.Mvc.CosmosDB.Services
{
    public class StarWarsCosmosDBClient
    {
        #region Fields
        private const string CHARACTERS_COLLECTION_ID = "CharactersCollection";

        private readonly CosmosDBOptions _options;
        private readonly DocumentClient _client;
        #endregion

        #region Properties
        public CosmosDBDocumentCollection<CharacterDocument> Characters { get; }
        #endregion

        #region Constructor
        public StarWarsCosmosDBClient(IOptions<CosmosDBOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _client = new DocumentClient(new Uri(_options.EndpointUri), _options.PrimaryKey);

            Characters = new CosmosDBDocumentCollection<CharacterDocument>(_client, _options.DatabaseId, CHARACTERS_COLLECTION_ID);
        }
        #endregion

        #region Methods
        public void EnsureDatabaseCreated()
        {
            _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _options.DatabaseId }).Wait();
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_options.DatabaseId), new DocumentCollection { Id = CHARACTERS_COLLECTION_ID }).Wait();
        }

        public void EnsureDatabaseSeeded()
        {
            Uri charactersCollectionUri = UriFactory.CreateDocumentCollectionUri(_options.DatabaseId, CHARACTERS_COLLECTION_ID);
            if (!_client.CreateDocumentQuery<CharacterDocument>(charactersCollectionUri, new FeedOptions { MaxItemCount = -1, }).Where(c => c.Id != "").Take(1).ToList().Any())
            {
                Task.WaitAll(
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Luke Skywalker", Gender = Genders.Male, Height = 172, Weight = 77, BirthYear = "19BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Blond, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "C-3PO", Height = 167, Weight = 75, BirthYear = "112BBY", SkinColor = SkinColors.Gold, EyeColor = EyeColors.Yellow, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "R2-D2", Height = 96, Weight = 32, BirthYear = "33BBY", SkinColor = SkinColors.Blue, EyeColor = EyeColors.Red, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Darth Vader", Gender = Genders.Male, Height = 202, Weight = 136, BirthYear = "41.9BBY", SkinColor = SkinColors.White, HairColor = HairColors.None, EyeColor = EyeColors.Yellow, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Leia Organa", Gender = Genders.Female, Height = 150, Weight = 49, BirthYear = "19BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Brown, EyeColor = EyeColors.Brown, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Owen Lars", Gender = Genders.Male, Height = 178, Weight = 120, BirthYear = "52BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Grey, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Beru Whitesun Lars", Gender = Genders.Female, Height = 165, Weight = 75, BirthYear = "47BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "R5-D4", Height = 97, Weight = 32, BirthYear = "Unknown", SkinColor = SkinColors.Red, EyeColor = EyeColors.Red, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Biggs Darklighter", Gender = Genders.Male, Height = 183, Weight = 84, BirthYear = "24BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Black, EyeColor = EyeColors.Brown, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Obi-Wan Kenobi", Gender = Genders.Male, Height = 182, Weight = 77, BirthYear = "57BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Auburn, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Anakin Skywalker", Gender = Genders.Male, Height = 188, Weight = 84, BirthYear = "41.9BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Wilhuff Tarkin", Gender = Genders.Male, Height = 180, BirthYear = "64BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Auburn, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Chewbacca", Gender = Genders.Male, Height = 228, Weight = 112, BirthYear = "200BBY", HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Han Solo", Gender = Genders.Male, Height = 180, Weight = 80, BirthYear = "29BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Brown, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Greedo", Gender = Genders.Male, Height = 173, Weight = 74, BirthYear = "44BBY", SkinColor = SkinColors.Green, EyeColor = EyeColors.Black, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Jabba Desilijic Tiure", Gender = Genders.Hermaphrodite, Height = 175, Weight = 1358, BirthYear = "600BBY", SkinColor = SkinColors.GreenTan, EyeColor = EyeColors.Orange, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Wedge Antilles", Gender = Genders.Male, Height = 170, Weight = 77, BirthYear = "21BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Hazel, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Jek Tono Porkins", Gender = Genders.Male, Height = 180, Weight = 110, BirthYear = "Unknown", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Yoda", Gender = Genders.Male, Height = 66, Weight = 17, BirthYear = "896BBY", SkinColor = SkinColors.Green, HairColor = HairColors.White, EyeColor = EyeColors.Brown, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Palpatine", Gender = Genders.Male, Height = 170, Weight = 75, BirthYear = "82BBY", SkinColor = SkinColors.Pale, HairColor = HairColors.Grey, EyeColor = EyeColors.Yellow, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow }),
                    _client.CreateDocumentAsync(charactersCollectionUri, new CharacterDocument { Id = Guid.NewGuid().ToString("N"), Name = "Boba Fett", Gender = Genders.Male, Height = 183, Weight = 78, BirthYear = "31.5BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Black, EyeColor = EyeColors.Brown, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow })
                );
            }
        }
        #endregion
    }
}
