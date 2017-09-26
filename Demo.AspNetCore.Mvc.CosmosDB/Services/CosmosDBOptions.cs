namespace Demo.AspNetCore.Mvc.CosmosDB.Services
{
    public class CosmosDBOptions
    {
        #region Properties
        public string EndpointUri { get; set; }

        public string PrimaryKey { get; set; }

        public string DatabaseId { get; set; }
        #endregion
    }
}
