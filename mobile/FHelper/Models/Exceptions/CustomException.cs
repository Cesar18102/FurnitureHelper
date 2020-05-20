using System;

using Newtonsoft.Json;

namespace Models.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CustomException : Exception
    {
        [JsonRequired]
        [JsonProperty("error_message")]
        private string message { get; set; }

        public override string Message => message;
    }
}
