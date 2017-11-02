using System;

namespace Demo.AspNetCore.Mvc.CosmosDB.Model
{
    interface IConditionalRequestMetadata
    {
        string EntityTag { get; }

        DateTime? LastModified { get; }
    }
}
