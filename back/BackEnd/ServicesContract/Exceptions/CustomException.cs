using System;

using Newtonsoft.Json;

namespace ServicesContract.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CustomException : Exception
    {
        [JsonProperty("error_message")]
        public override string Message => base.Message;
    }
}
