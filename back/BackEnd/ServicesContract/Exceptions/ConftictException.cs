using Newtonsoft.Json;

namespace ServicesContract.Exceptions
{
    public class ConftictException : CustomException
    {
        [JsonProperty("conflict_on")]
        private string ConflictSubject { get; set; }

        public override string Message => $"{ConflictSubject} conflict";

        public ConftictException(string conflictSubject)
        {
            ConflictSubject = conflictSubject;
        }
    }
}
