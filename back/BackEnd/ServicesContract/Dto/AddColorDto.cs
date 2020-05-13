using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddColorDto : IDto
    {
        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "red is required")]
        [Range(0, 255, ErrorMessage = "red must be an integer between 0 and 255")]
        [JsonProperty("red")]
        public int? Red { get; set; }

        [Required(ErrorMessage = "green is required")]
        [Range(0, 255, ErrorMessage = "green must be an integer between 0 and 255")]
        [JsonProperty("green")]
        public int? Green { get; set; }

        [Required(ErrorMessage = "blue is required")]
        [Range(0, 255, ErrorMessage = "blue must be an integer between 0 and 255")]
        [JsonProperty("blue")]
        public int? Blue { get; set; }

        [Required(ErrorMessage = "alpha is required")]
        [Range(0, 255, ErrorMessage = "alpha must be an integer between 0 and 255")]
        [JsonProperty("alpha")]
        public int? Alpha { get; set; }
    }
}
