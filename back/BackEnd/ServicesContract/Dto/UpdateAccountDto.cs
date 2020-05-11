using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateAccountDto : IDto
    {
        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [RegularExpression("^\\w{8,64}$", ErrorMessage = "password is invalid")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+(\\-[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+)*$", ErrorMessage = "first_name is invalid")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [RegularExpression("^[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+(\\-[A-Za-zА-Яа-яІіЇїЄєЙйЁё]+)*$", ErrorMessage = "last_name is invalid")]
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
