using System;

using Newtonsoft.Json;

namespace Models
{
    public class SessionModel : IModel
    {
        [JsonProperty("user_id")]
        public int UserId { get; private set; }

        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; private set; }

        public SessionModel(int userId, string token, DateTime expires)
        {
            UserId = userId;
            Token = token;
            Expires = expires;
        }
    }
}
