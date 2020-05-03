using System;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateAccountDto : IDto
    {
        [JsonProperty("session")]
        public SessionDto Session { get; private set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public void Validate()
        {
            
        }
    }
}
