using Newtonsoft.Json;

namespace Models.Exceptions
{
    public class ForbiddenException : CustomException
    {
        [JsonRequired]
        [JsonProperty("needed_rights_level")]
        private string NeededRightsLevel { get; set; }
    }
}
