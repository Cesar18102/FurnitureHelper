using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddColorDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "red is required")]
        [Range(0, 255)]
        [JsonProperty("red")]
        public int Red { get; set; }

        [Required(ErrorMessage = "green is required")]
        [Range(0, 255)]
        [JsonProperty("green")]
        public int Green { get; set; }

        [Required(ErrorMessage = "blue is required")]
        [Range(0, 255)]
        [JsonProperty("blue")]
        public int Blue { get; set; }

        public virtual void Validate()
        {
            
        }
    }
}
