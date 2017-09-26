using System;
using Newtonsoft.Json;
using Demo.AspNetCore.Mvc.CosmosDB.Model;

namespace Demo.AspNetCore.Mvc.CosmosDB.Documents
{
    public class CharacterDocument : Character, IDocument
    {
        [JsonProperty(PropertyName = "id")]
        public new string Id
        {
            get { return base.Id; }

            set { base.Id = value; }
        }

        public new DateTime CreatedDate
        {
            get { return base.CreatedDate; }

            set { base.CreatedDate = value; }
        }

        public new DateTime LastUpdatedDate
        {
            get { return base.LastUpdatedDate; }

            set { base.LastUpdatedDate = value; }
        }
    }
}
