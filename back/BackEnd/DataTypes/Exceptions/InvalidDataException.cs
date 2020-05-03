using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace DataTypes.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class InvalidDataException<T> : Exception where T : IDto
    {
        [JsonProperty("invalid_info")]
        public List<InvalidFieldInfo<T>> InvalidFieldInfos { get; private set; } = new List<InvalidFieldInfo<T>>();

        [JsonProperty("error_message")]
        public override string Message => "Validation failed";
    }

    public class InvalidFieldInfo<T> where T : IDto
    {
        [JsonProperty("field_name")]
        public string FieldName { get; private set; }

        [JsonProperty("invalid_reason")]
        public string InvalidReason { get; private set; }

        public InvalidFieldInfo(string fieldName, string invalidReason)
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
