using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class SignUpDto : IDto
    {
        [Required(ErrorMessage = "login is required")]
        [RegularExpression("^\\w{5,64}$", ErrorMessage = "login is invalid")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "email is invalid")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password is required")]
        [RegularExpression("^\\w{8,64}$", ErrorMessage = "password is invalid")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "first_name is required")]
        [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+(\\-[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+)*$", ErrorMessage = "first_name is invalid")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "last_name is required")]
        [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+(\\-[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+)*$", ErrorMessage = "last_name is invalid")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public void Validate()
        {
            
        }
    }
}