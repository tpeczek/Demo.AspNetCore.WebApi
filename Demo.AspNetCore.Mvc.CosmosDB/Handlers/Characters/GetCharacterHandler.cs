using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.AspNetCore.Mvc.CosmosDB.Model;
using Demo.AspNetCore.Mvc.CosmosDB.Requests;
using Demo.AspNetCore.Mvc.CosmosDB.Services;

namespace Demo.AspNetCore.Mvc.CosmosDB.Handlers.Characters
{
    public class GetCharacterHandler : IGetSingleHandler<Character>
    {
        #region Fields
        private readonly StarWarsCosmosDBClient _client;
        #endregion

        #region Constructor
        public GetCharacterHandler(StarWarsCosmosDBClient client)
        {
            _client = client;
        }
        #endregion

        #region Methods
        public async Task<Character> Handle(GetSingleRequest<Character> request, CancellationToken cancellationToken)
        {
            return await _client.Characters.GetDocumentQuery().Where(c => c.Id == request.Id).ToAsyncEnumerable().FirstOrDefault();
        }
        #endregion
    }
}
