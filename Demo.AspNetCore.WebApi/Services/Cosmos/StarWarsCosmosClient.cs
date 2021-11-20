using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Demo.AspNetCore.WebApi.Characters;

namespace Demo.AspNetCore.WebApi.Services.Cosmos
{
    internal class StarWarsCosmosClient : IStarWarsCosmosClient, IDisposable
    {
        #region Fields
        private const string CHARACTERS_CONTAINER_ID = "Characters";
        private static readonly string CHARACTERS_CONTAINER_PARTITION_KEY_PATH = "/partitionKeyNone";

        private readonly CosmosOptions _cosmosOptions;
        private readonly CosmosClient _cosmosClient;
        private bool _cosmosClientDisposed;
        #endregion

        #region Properties
        public Container Characters { get; }
        #endregion

        #region Constructor
        public StarWarsCosmosClient(IOptions<CosmosOptions> comosOptionsRetriever)
        {
            _cosmosOptions = comosOptionsRetriever.Value;

            _cosmosClient = new CosmosClient(_cosmosOptions.EndpointUri, _cosmosOptions.PrimaryKey, new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                }
            });

            Characters = _cosmosClient.GetContainer(_cosmosOptions.DatabaseId, CHARACTERS_CONTAINER_ID);
        }
        #endregion

        #region Methods
        public async Task EnsureDatabaseAndContainerExistsAsync()
        {
            DatabaseResponse databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_cosmosOptions.DatabaseId);
            if (databaseResponse.StatusCode != HttpStatusCode.OK && databaseResponse.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Database {_cosmosOptions.DatabaseId} doesn't exists and couldn't be created.");
            }

            ContainerResponse containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(CHARACTERS_CONTAINER_ID, CHARACTERS_CONTAINER_PARTITION_KEY_PATH);
            if (containerResponse.StatusCode != HttpStatusCode.OK && containerResponse.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Container {CHARACTERS_CONTAINER_ID} doesn't exists and couldn't be created.");
            }
        }

        public async Task EnsureDatabaseSeededAsync()
        {
            Container charactersContainer = _cosmosClient.GetContainer(_cosmosOptions.DatabaseId, CHARACTERS_CONTAINER_ID);
            using FeedIterator<Character> charactersFeedIterator = charactersContainer.GetItemQueryIterator<Character>($"SELECT * FROM C");
            FeedResponse<Character> charactersFeedResponse = await charactersFeedIterator.ReadNextAsync();
            if (charactersFeedResponse.Count == 0)
            {
                await Task.WhenAll(
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Luke Skywalker", Gender = Genders.Male, Height = 172, Weight = 77, BirthYear = "19BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Blond, EyeColor = EyeColors.Blue, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "C-3PO", Height = 167, Weight = 75, BirthYear = "112BBY", SkinColor = SkinColors.Gold, EyeColor = EyeColors.Yellow, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "R2-D2", Height = 96, Weight = 32, BirthYear = "33BBY", SkinColor = SkinColors.Blue, EyeColor = EyeColors.Red, Homeworld = "Naboo" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Darth Vader", Gender = Genders.Male, Height = 202, Weight = 136, BirthYear = "41.9BBY", SkinColor = SkinColors.White, HairColor = HairColors.None, EyeColor = EyeColors.Yellow, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Leia Organa", Gender = Genders.Female, Height = 150, Weight = 49, BirthYear = "19BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Brown, EyeColor = EyeColors.Brown, Homeworld = "Alderaan" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Owen Lars", Gender = Genders.Male, Height = 178, Weight = 120, BirthYear = "52BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Grey, EyeColor = EyeColors.Blue, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Beru Whitesun Lars", Gender = Genders.Female, Height = 165, Weight = 75, BirthYear = "47BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "R5-D4", Height = 97, Weight = 32, BirthYear = "Unknown", SkinColor = SkinColors.Red, EyeColor = EyeColors.Red, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Biggs Darklighter", Gender = Genders.Male, Height = 183, Weight = 84, BirthYear = "24BBY", SkinColor = SkinColors.Light, HairColor = HairColors.Black, EyeColor = EyeColors.Brown, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Obi-Wan Kenobi", Gender = Genders.Male, Height = 182, Weight = 77, BirthYear = "57BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Auburn, EyeColor = EyeColors.Blue, Homeworld = "Stewjon" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Anakin Skywalker", Gender = Genders.Male, Height = 188, Weight = 84, BirthYear = "41.9BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, Homeworld = "Tatooine" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Wilhuff Tarkin", Gender = Genders.Male, Height = 180, BirthYear = "64BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Auburn, EyeColor = EyeColors.Blue, Homeworld = "Eriadu" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Chewbacca", Gender = Genders.Male, Height = 228, Weight = 112, BirthYear = "200BBY", HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, Homeworld = "Kashyyyk" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Han Solo", Gender = Genders.Male, Height = 180, Weight = 80, BirthYear = "29BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Brown, Homeworld = "Corellia" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Greedo", Gender = Genders.Male, Height = 173, Weight = 74, BirthYear = "44BBY", SkinColor = SkinColors.Green, EyeColor = EyeColors.Black, Homeworld = "Rodia" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Jabba Desilijic Tiure", Gender = Genders.Hermaphrodite, Height = 175, Weight = 1358, BirthYear = "600BBY", SkinColor = SkinColors.GreenTan, EyeColor = EyeColors.Orange, Homeworld = "Nal Hutta" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Wedge Antilles", Gender = Genders.Male, Height = 170, Weight = 77, BirthYear = "21BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Hazel, Homeworld = "Corellia" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Jek Tono Porkins", Gender = Genders.Male, Height = 180, Weight = 110, BirthYear = "Unknown", SkinColor = SkinColors.Fair, HairColor = HairColors.Brown, EyeColor = EyeColors.Blue, Homeworld = "Bestine IV" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Yoda", Gender = Genders.Male, Height = 66, Weight = 17, BirthYear = "896BBY", SkinColor = SkinColors.Green, HairColor = HairColors.White, EyeColor = EyeColors.Brown, Homeworld = "Unknown" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Palpatine", Gender = Genders.Male, Height = 170, Weight = 75, BirthYear = "82BBY", SkinColor = SkinColors.Pale, HairColor = HairColors.Grey, EyeColor = EyeColors.Yellow, Homeworld = "Naboo" }),
                    charactersContainer.CreateItemAsync(new Character { Id = Guid.NewGuid().ToString("N"), Name = "Boba Fett", Gender = Genders.Male, Height = 183, Weight = 78, BirthYear = "31.5BBY", SkinColor = SkinColors.Fair, HairColor = HairColors.Black, EyeColor = EyeColors.Brown, Homeworld = "Kamino" })
                );
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_cosmosClientDisposed)
            {
                if (disposing)
                {
                    _cosmosClient.Dispose();
                }

                _cosmosClientDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
