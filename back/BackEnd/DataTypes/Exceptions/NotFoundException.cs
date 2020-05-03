using System;

using Newtonsoft.Json;

namespace DataTypes.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class NotFoundException : Exception
    {
        [JsonProperty("not_found")]
        private string NotFoundSubject { get; set; }

        [JsonProperty("error_message")]
        public override string Message => $"{NotFoundSubject} not found";

        public NotFoundException(string notFoundSubject)
        {
            NotFoundSubject = notFoundSubject;
        }
    }
}
