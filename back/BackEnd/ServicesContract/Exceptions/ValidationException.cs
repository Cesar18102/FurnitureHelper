using System.Reflection;
using System.Collections.Generic;

using Newtonsoft.Json;

using ServicesContract.Dto;
using System;

namespace ServicesContract.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ValidationException : CustomException
    {
        [JsonProperty("validation_fail_info")]
        public List<ValidationFailInfo> ValidationFailInfos { get; private set; } = new List<ValidationFailInfo>();

        public override string Message => "Validation failed";
    }

    public class ValidationFailInfo
    {
        [JsonProperty("field_name")]
        public string FieldName { get; private set; }

        [JsonProperty("invalid_reason")]
        public string InvalidReason { get; private set; }

        private ValidationFailInfo() { }

        public static ValidationFailInfo CreateValidationFailInfo<T>(string fieldName, string invalidReason) where T : IDto
        {
            return CreateValidationFailInfo(typeof(T), fieldName, invalidReason);
        }

        public static ValidationFailInfo CreateValidationFailInfo(Type type, string fieldName, string invalidReason)
        {
            if (!typeof(IDto).IsAssignableFrom(type))
                throw new ArgumentException();

            ValidationFailInfo failInfo = new ValidationFailInfo();

            failInfo.FieldName = fieldName;
            failInfo.InvalidReason = invalidReason;

            PropertyInfo propInfo = type.GetProperty(fieldName);
            JsonPropertyAttribute attribute = propInfo.GetCustomAttribute<JsonPropertyAttribute>();

            if (attribute != null)
            {
                failInfo.InvalidReason = failInfo.InvalidReason.Replace(failInfo.FieldName, attribute.PropertyName);
                failInfo.FieldName = attribute.PropertyName;
            }

            return failInfo;
        }

        public override string ToString()
        {
            return $"{FieldName}: {InvalidReason}";
        }
    }
}
