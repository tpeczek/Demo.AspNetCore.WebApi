using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MediatR;
using Demo.AspNetCore.WebApi.Services.Cosmos;

namespace Demo.AspNetCore.WebApi.Characters.Actions
{
    internal class DeleteRequest : IRequest
    {
        #region Properties
        public string Id { get; }
        #endregion

        #region Constructor
        public DeleteRequest(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"The {nameof(id)} needs to have a non-white-space value.", nameof(id));
            }

            Id = id;
        }
        #endregion
    }

    internal class DeleteHandler : IRequestHandler<DeleteRequest>
    {
        #region Fields
        private readonly IStarWarsCosmosClient _starWarsCosmosClient;
        #endregion

        #region Constructor
        public DeleteHandler(IStarWarsCosmosClient starWarsCosmosClient)
        {
            _starWarsCosmosClient = starWarsCosmosClient;
        }
        #endregion

        #region Methods
        public async Task<Unit> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            await _starWarsCosmosClient.Characters.DeleteItemAsync<Character>(request.Id, PartitionKey.None);

            return Unit.Value;
        }
        #endregion
    }
}
