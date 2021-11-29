using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class ReplaceRequest : IRequest<Character>
    {
        #region Properties
        public string Id { get; }

        public Character Character { get; }

        public Character Replacement { get; }
        #endregion

        #region Constructor
        public ReplaceRequest(Character character, Character replacement)
        {
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Replacement = replacement ?? throw new ArgumentNullException(nameof(replacement));
        }
        #endregion
    }

    internal class ReplaceHandler : IRequestHandler<ReplaceRequest, Character>
    {
        #region Fields
        private readonly IMediator _mediator;
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public ReplaceHandler(IMediator mediator, IStarWarsCosmosClient starWarsCosmosClient)
        {
            _mediator = mediator;
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(ReplaceRequest request, CancellationToken cancellationToken)
        {
            Character character = request.Character;

            character.Name = request.Replacement.Name;
            character.Gender = request.Replacement.Gender;
            character.Height = request.Replacement.Height;
            character.Weight = request.Replacement.Weight;
            character.BirthYear = request.Replacement.BirthYear;
            character.SkinColor = request.Replacement.SkinColor;
            character.HairColor = request.Replacement.HairColor;
            character.EyeColor = request.Replacement.EyeColor;
            character.Homeworld = request.Replacement.Homeworld;

            ItemResponse<Character> characterItemResponse = await _starWarsCosmosClient.Characters.ReplaceItemAsync(character, character.Id);

            character = characterItemResponse.Resource;
            (character as IConditionalRequestMetadata).SetValidatros(new ConditionalRequestValidators(characterItemResponse.ETag, null));

            return character;
        }
        #endregion
    }
}
