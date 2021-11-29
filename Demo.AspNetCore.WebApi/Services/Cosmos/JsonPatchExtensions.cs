using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Demo.AspNetCore.WebApi.Http;

namespace Demo.AspNetCore.WebApi.Services.Cosmos
{
    public static class JsonPatchExtensions
    {
        private static MethodInfo _createAddPatchOperationMethodInfo = typeof(JsonPatchExtensions).GetMethod(nameof(JsonPatchExtensions.CreateAddPatchOperation), BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo _createReplacePatchOperationMethodInfo = typeof(JsonPatchExtensions).GetMethod(nameof(JsonPatchExtensions.CreateReplacePatchOperation), BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo _createSetPatchOperationMethodInfo = typeof(JsonPatchExtensions).GetMethod(nameof(JsonPatchExtensions.CreateSetPatchOperation), BindingFlags.NonPublic | BindingFlags.Static);

        public static IReadOnlyList<PatchOperation> ToCosmosPatchOperations<T>(this JsonPatch<T> jsonPatchOperations)
        {
            List<PatchOperation> cosmosPatchOperations = new List<PatchOperation>(jsonPatchOperations.Count);
            foreach (JsonPatchOperation jsonPatchOperation in jsonPatchOperations)
            {
                switch (jsonPatchOperation.OperationType)
                {
                    case JsonPatchOperationType.Add:
                        cosmosPatchOperations.Add(CreatePatchOperation(_createAddPatchOperationMethodInfo, jsonPatchOperations, jsonPatchOperation));
                        break;
                    case JsonPatchOperationType.Remove:
                        cosmosPatchOperations.Add(PatchOperation.Remove(jsonPatchOperation.Path));
                        break;
                    case JsonPatchOperationType.Replace:
                        cosmosPatchOperations.Add(CreatePatchOperation(_createReplacePatchOperationMethodInfo, jsonPatchOperations, jsonPatchOperation));
                        break;
                    case JsonPatchOperationType.Set:
                        cosmosPatchOperations.Add(CreatePatchOperation(_createSetPatchOperationMethodInfo, jsonPatchOperations, jsonPatchOperation));
                        break;
                }
            }

            return cosmosPatchOperations;
        }

        private static PatchOperation CreateAddPatchOperation<T>(JsonPatchOperation jsonPatchOperation)
        {
            return PatchOperation.Add(jsonPatchOperation.Path, jsonPatchOperation.GetValue<T>());
        }

        private static PatchOperation CreateReplacePatchOperation<T>(JsonPatchOperation jsonPatchOperation)
        {
            return PatchOperation.Replace(jsonPatchOperation.Path, jsonPatchOperation.GetValue<T>());
        }

        private static PatchOperation CreateSetPatchOperation<T>(JsonPatchOperation jsonPatchOperation)
        {
            return PatchOperation.Set(jsonPatchOperation.Path, jsonPatchOperation.GetValue<T>());
        }

        private static PatchOperation CreatePatchOperation<T>(MethodInfo createSpecificPatchOperationMethodInfo, JsonPatch<T> jsonPatchOperations, JsonPatchOperation jsonPatchOperation)
        {
            Type jsonPatchOperationValueType = jsonPatchOperations.GetTypeForPath(jsonPatchOperation.Path);

            MethodInfo createSpecificPatchOperationWithValueTypeMethodInfo = createSpecificPatchOperationMethodInfo.MakeGenericMethod(jsonPatchOperationValueType);

            return (PatchOperation)createSpecificPatchOperationWithValueTypeMethodInfo.Invoke(null, new object[] { jsonPatchOperation });
        }
    }
}
