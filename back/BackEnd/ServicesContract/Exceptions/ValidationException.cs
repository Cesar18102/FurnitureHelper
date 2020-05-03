using System.Collections.Generic;

using Newtonsoft.Json;

using Models;

namespace ServicesContract.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ValidationException<T> : CustomException where T : IModel
    {
        [JsonProperty("validation_fail_info")]
        public List<ValidationFailInfo<T>> ValidationFailInfos { get; private set; } = new List<ValidationFailInfo<T>>();

        public override string Message => "Validation failed";
    }

    public class ValidationFailInfo<T> where T : IModel
    {
        [JsonProperty("field_name")]
        public string FieldName { get; private set; }

        [JsonProperty("invalid_reason")]
        public string InvalidReason { get; private set; }

        public ValidationFailInfo(string fieldName, string invalidReason)
        {
            FieldName = fieldName;
            InvalidReason = invalidReason;
        }

        public override string ToString()
        {
            return $"{FieldName}: {InvalidReason}";
        }
    }
}
