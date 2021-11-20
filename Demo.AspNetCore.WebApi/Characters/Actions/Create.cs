using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class CreateRequest : IRequest<Character>
    {
        #region Properties
        public Character Character { get; }
        #endregion

        #region Constructor
        public CreateRequest(Character character)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
        }
        #endregion
    }

    internal class CreateHandler : IRequestHandler<CreateRequest, Character>
    {
        #region Fields
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public CreateHandler(IStarWarsCosmosClient starWarsCosmosClient)
        {
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(request.Character.Id))
            {
                request.Character.Id = Guid.NewGuid().ToString("N");
            }

            ItemResponse<Character> characterItemResponse = await _starWarsCosmosClient.Characters.CreateItemAsync(request.Character);

            Character character = characterItemResponse.Resource;
            (character as IConditionalRequestMetadata).SetValidatros(new ConditionalRequestValidators(characterItemResponse.ETag, null));

            return character;
        }
        #endregion
    }
}
