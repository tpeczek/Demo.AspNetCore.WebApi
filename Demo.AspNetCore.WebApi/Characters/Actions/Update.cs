using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;
using Demo.AspNetCore.WebApi.Http.ConditionalRequests;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class UpdateRequest : IRequest<Character>
    {
        #region Properties
        public string Id { get; }

        public Character Character { get; }

        public RequestConditions Conditions { get; }
        #endregion

        #region Constructor
        public UpdateRequest(string id, Character character, RequestConditions conditions)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"The {nameof(id)} needs to have a non-white-space value.", nameof(id));
            }

            Id = id;
            Character = character ?? throw new ArgumentNullException(nameof(character));
            Conditions = conditions;
        }
        #endregion
    }

    internal class UpdateHandler : IRequestHandler<UpdateRequest, Character>
    {
        #region Fields
        private readonly IMediator _mediator;
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public UpdateHandler(IMediator mediator, IStarWarsCosmosClient starWarsCosmosClient)
        {
            _mediator = mediator;
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            Character character = await _mediator.Send(new GetByIdRequest(request.Id), cancellationToken);

            request.Conditions?.CheckPreconditionFailed((character as IConditionalRequestMetadata).GetValidators());

            character.Name = request.Character.Name;
            character.Gender = request.Character.Gender;
            character.Height = request.Character.Height;
            character.Weight = request.Character.Weight;
            character.BirthYear = request.Character.BirthYear;
            character.SkinColor = request.Character.SkinColor;
            character.HairColor = request.Character.HairColor;
            character.EyeColor = request.Character.EyeColor;
            character.Homeworld = request.Character.Homeworld;

            ItemResponse<Character> characterItemResponse = await _starWarsCosmosClient.Characters.ReplaceItemAsync(character, character.Id);

            character = characterItemResponse.Resource;
            (character as IConditionalRequestMetadata).SetValidatros(new ConditionalRequestValidators(characterItemResponse.ETag, null));

            return character;
        }
        #endregion
    }
}
