using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.AspNetCore.WebApi.Http
{
    public enum JsonPatchOperationType
    {
        Add,
        Set,
        Remove,
        Replace,
        Invalid
    }

    public class JsonPatchOperation : IValidatableObject
    {
        #region Fields
        private string _op;
        private JsonPatchOperationType _operationType;
        #endregion

        #region Properties
        [Required]
        public string Op
        {
            get { return _op; }

            set
            {
                JsonPatchOperationType operationType;
                if (!Enum.TryParse(value, ignoreCase: true, result: out operationType))
                {
                    operationType = JsonPatchOperationType.Invalid;
                }

                _operationType = operationType;

                _op = value;
            }
        }

        [Required]
        public string Path { get; set; }

        public JsonPatchOperationType OperationType => _operationType;

        public object Value { get; set; }
        #endregion

        #region Methods
        public T GetValue<T>()
        {
            return ((JsonElement)Value).Deserialize<T>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OperationType == JsonPatchOperationType.Invalid)
            {
                yield return new ValidationResult($"Not supported operation: {Op}.", new[] { nameof(Op) });
            }
        }
        #endregion
    }

    public class JsonPatch<T> : List<JsonPatchOperation>
    {
        private static readonly IDictionary<string, Type> _pathsTypes;

        static JsonPatch()
        {
            _pathsTypes = typeof(T).GetProperties().ToDictionary(p => $"/{Char.ToLowerInvariant(p.Name[0]) + p.Name[1..]}", p => p.PropertyType);
        }

        public Type GetTypeForPath(string path)
        {
            return _pathsTypes[path];
        }
    }
}
