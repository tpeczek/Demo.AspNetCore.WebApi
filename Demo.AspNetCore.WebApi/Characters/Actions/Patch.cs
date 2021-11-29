using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Http;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;
using Demo.AspNetCore.WebApi.Services.Cosmos;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class PatchRequest : IRequest<Character>
    {
        #region Properties
        public string Id { get; }

        public JsonPatch<Character> Operations { get; }
        #endregion

        #region Constructor
        public PatchRequest(string id, JsonPatch<Character> operations)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"The {nameof(id)} needs to have a non-white-space value.", nameof(id));
            }

            Id = id;
            Operations = operations ?? throw new ArgumentNullException(nameof(operations));
        }
        #endregion
    }

    internal class PatchHandler : IRequestHandler<PatchRequest, Character>
    {
        #region Fields
        private readonly IMediator _mediator;
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public PatchHandler(IMediator mediator, IStarWarsCosmosClient starWarsCosmosClient)
        {
            _mediator = mediator;
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(PatchRequest request, CancellationToken cancellationToken)
        {
            ItemResponse<Character> characterItemResponse = await _starWarsCosmosClient.Characters.PatchItemAsync<Character>(request.Id, PartitionKey.None, request.Operations.ToCosmosPatchOperations());

            Character character = characterItemResponse.Resource;
            (character as IConditionalRequestMetadata).SetValidatros(new ConditionalRequestValidators(characterItemResponse.ETag, null));

            return character;
        }
        #endregion
    }
}
