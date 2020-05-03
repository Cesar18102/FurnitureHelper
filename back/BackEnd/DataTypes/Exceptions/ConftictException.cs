using System;

using Newtonsoft.Json;

namespace DataTypes.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ConftictException : Exception
    {
        [JsonProperty("conflict_on")]
        private string ConflictSubject { get; set; }

        [JsonProperty("error_message")]
        public override string Message => $"{ConflictSubject} conflict";

        public ConftictException(string conflictSubject)
        {
            ConflictSubject = conflictSubject;
        }
    }
}
