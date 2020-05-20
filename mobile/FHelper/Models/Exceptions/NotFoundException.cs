using Newtonsoft.Json;

namespace Models.Exceptions
{
    public class NotFoundException : CustomException
    {
        [JsonRequired]
        [JsonProperty("not_found")]
        private string NotFoundSubject { get; set; }
    }
}
