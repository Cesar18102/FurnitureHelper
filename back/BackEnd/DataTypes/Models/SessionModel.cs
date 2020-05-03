using System;

using Newtonsoft.Json;

namespace Models
{
    public class SessionModel : IModel
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; private set; }

        public SessionModel(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
        }
    }
}
