using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class SessionDto : IDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "user_id is required")]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "session_token_salted is required")]
        [JsonProperty("session_token_salted")]
        public string SessionTokenSalted { get; set; }

        [Required(ErrorMessage = "salt is required")]
        [JsonProperty("salt")]
        public string Salt { get; set; }

        public void Validate()
        {
            
        }
    }
}
