using System;

namespace Demo.AspNetCore.WebApi.Http.ConditionalRequests
{
    internal interface IConditionalRequestMetadata
    {
        internal ConditionalRequestValidators GetValidators();

        internal void SetValidatros(ConditionalRequestValidators validators);
        
    }
}
