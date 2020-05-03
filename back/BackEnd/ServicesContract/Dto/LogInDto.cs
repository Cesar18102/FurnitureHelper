using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class LogInDto : IDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password_salted")]
        public string PasswordSalted { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }

        public void Validate()
        {
            
        }
    }
}
