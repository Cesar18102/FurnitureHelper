using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateColorDto : IDto
    {
        [Required(ErrorMessage = "id is required")]
        [Range(1, int.MaxValue)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Range(0, 255)]
        [JsonProperty("red")]
        public int Red { get; set; }

        [Range(0, 255)]
        [JsonProperty("green")]
        public int Green { get; set; }

        [Range(0, 255)]
        [JsonProperty("blue")]
        public int Blue { get; set; }

        public void Validate()
        {
            
        }
    }
}
