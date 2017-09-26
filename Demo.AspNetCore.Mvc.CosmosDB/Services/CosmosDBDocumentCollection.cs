using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Demo.AspNetCore.Mvc.CosmosDB.Documents;

namespace Demo.AspNetCore.Mvc.CosmosDB.Services
{
    public class CosmosDBDocumentCollection<T> where T : IDocument
    {
        #region Fields
        private readonly DocumentClient _client;

        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly Uri _collectionUri;

        private static readonly FeedOptions _feedOptions = new FeedOptions { MaxItemCount = -1, };
        #endregion

        #region Constructor
        public CosmosDBDocumentCollection(DocumentClient client, string databaseId, string collectionId)
        {
            _client = client;

            _databaseId = databaseId;
            _collectionId = collectionId;
            _collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
        }
        #endregion

        #region Methods
        public IQueryable<T> GetDocumentQuery()
        {
            return _client.CreateDocumentQuery<T>(_collectionUri, _feedOptions);
        }

        public Task CreateDocumentAsync(T document)
        {
            return _client.CreateDocumentAsync(_collectionUri, document);
        }

        public Task ReplaceDocumentAsync(T document)
        {
            return _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, document.Id), document);
        }

        public Task DeleteDocumentAsync(string documentId)
        {
            return _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId));
        }
        #endregion
    }
}
