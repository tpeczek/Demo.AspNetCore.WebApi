using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class GetAllRequest : IRequest<IAsyncEnumerable<Character>>
    { }

    internal class GetAllHandler : IRequestHandler<GetAllRequest, IAsyncEnumerable<Character>>
    {
        #region Fields
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public GetAllHandler(IStarWarsCosmosClient starWarsCosmosClient)
        {
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public Task<IAsyncEnumerable<Character>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(GetAllInternal());
        }

        private async IAsyncEnumerable<Character> GetAllInternal()
        {
            using FeedIterator<Character> charactersFeedIterator = _starWarsCosmosClient.Characters.GetItemQueryIterator<Character>("SELECT * FROM C");
            while (charactersFeedIterator.HasMoreResults)
            {
                FeedResponse<Character> charactersFeedResponse = await charactersFeedIterator.ReadNextAsync();
                foreach (Character character in charactersFeedResponse.Resource)
                {
                    yield return character;
                }
            }
        }
        #endregion
    }
}
