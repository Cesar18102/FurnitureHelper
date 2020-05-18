using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class BuildSessionDto : IDto
    {
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [JsonProperty("build_session_token")]
        public string BuildSessionToken { get; set; }
    }
}
