using Newtonsoft.Json;

namespace Models.Exceptions
{
    public class ConflictException : CustomException
    {
        [JsonRequired]
        [JsonProperty("conflict_on")]
        private string ConflictSubject { get; set; }
    }
}
