using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class GetByIdRequest : IRequest<Character>
    {
        #region Properties
        public string Id { get; }
        #endregion

        #region Constructor
        public GetByIdRequest(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"The {nameof(id)} needs to have a non-white-space value.", nameof(id));
            }

            Id = id;
        }
        #endregion
    }

    internal class GetByIdHandler : IRequestHandler<GetByIdRequest, Character>
    {
        #region Fields
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public GetByIdHandler(IStarWarsCosmosClient starWarsCosmosClient)
        {
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                ItemResponse<JObject> characterItemResponse = await _starWarsCosmosClient.Characters.ReadItemAsync<JObject>(request.Id, PartitionKey.None);

                Character character = characterItemResponse.Resource.ToObject<Character>();
                (character as IConditionalRequestMetadata).SetValidatros(new ConditionalRequestValidators(
                    characterItemResponse.ETag,
                    DateTimeOffset.FromUnixTimeSeconds(characterItemResponse.Resource.Value<long>("_ts")).DateTime
                ));

                return character;
            }
            catch (CosmosException cex)
            {
                if (cex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }
        #endregion
    }
}
