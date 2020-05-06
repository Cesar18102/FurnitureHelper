using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateColorDto : IDto
    {
        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

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

        public void Validate()
        {
            
        }
    }
}
