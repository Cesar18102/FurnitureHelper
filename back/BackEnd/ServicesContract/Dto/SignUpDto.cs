using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class SignUpDto : IDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

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