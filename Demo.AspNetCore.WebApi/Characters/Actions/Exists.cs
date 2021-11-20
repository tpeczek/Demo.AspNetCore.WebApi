using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Demo.AspNetCore.WebApi.Services.Cosmos;
using MediatR;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class ExistsRequest : IRequest<bool>
    {
        #region Properties
        public string Name { get; }

        public string OtherThanId { get; }
        #endregion

        #region Constructor
        public ExistsRequest(string name)
            : this(name, null)
        { }

        public ExistsRequest(string name, string otherThanId)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The {nameof(name)} needs to have a non-white-space value.", nameof(name));
            }

            Name = name;
            OtherThanId = otherThanId;
        }
        #endregion
    }

    internal class ExistsHandler : IRequestHandler<ExistsRequest, bool>
    {
        #region Fields
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public ExistsHandler(IStarWarsCosmosClient starWarsCosmosClient)
        {
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<bool> Handle(ExistsRequest request, CancellationToken cancellationToken)
        {
            string queryText = String.IsNullOrWhiteSpace(request.OtherThanId) ? $"SELECT * FROM C WHERE C.name = '{request.Name}'" : $"SELECT * FROM C WHERE C.id != '{request.OtherThanId}' AND C.name = '{request.Name}'";
            using FeedIterator<Character> charactersFeedIterator = _starWarsCosmosClient.Characters.GetItemQueryIterator<Character>(queryText);

            return (charactersFeedIterator.HasMoreResults && (await charactersFeedIterator.ReadNextAsync()).Count != 0);
        }
        #endregion
    }
}
