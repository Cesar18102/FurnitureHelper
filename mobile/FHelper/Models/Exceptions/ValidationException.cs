using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Exceptions
{
    public class ValidationException : CustomException
    {
        [JsonRequired]
        [JsonProperty("validation_fail_info")]
        public List<ValidationFailInfo> ValidationFailInfos { get; private set; } = new List<ValidationFailInfo>();
    }

    public class ValidationFailInfo
    {
        [JsonRequired]
        [JsonProperty("field_name")]
        public string FieldName { get; private set; }

        [JsonRequired]
        [JsonProperty("invalid_reason")]
        public string InvalidReason { get; private set; }
    }
}
