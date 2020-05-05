using Newtonsoft.Json;

namespace ServicesContract.Exceptions
{
    public class ConflictException : CustomException
    {
        [JsonProperty("conflict_on")]
        private string ConflictSubject { get; set; }

        public override string Message => $"{ConflictSubject} conflict";

        public ConflictException(string conflictSubject)
        {
            ConflictSubject = conflictSubject;
        }
    }
}
