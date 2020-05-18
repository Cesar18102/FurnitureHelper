using Newtonsoft.Json;

namespace Models
{
    public class BuildSessionModel : IModel
    {
        [JsonProperty("build_session_token")]
        public string BuildSessionToken { get; private set; }

        public BuildSessionModel(string token)
        {
            BuildSessionToken = token;
        }
    }
}
