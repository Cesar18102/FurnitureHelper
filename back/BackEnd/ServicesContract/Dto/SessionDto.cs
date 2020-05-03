using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class SessionDto : IDto
    {
        [JsonProperty("session_token_salted")]
        public string SessionTokenSalted { get; private set; }

        [JsonProperty("salt")]
        public string Salt { get; private set; }

        public void Validate()
        {
            
        }
    }
}
