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

        [Range(0, 255, ErrorMessage = "red is required between 0 and 255")]
        [JsonProperty("red")]
        public int Red { get; set; } = -1;

        [Range(0, 255, ErrorMessage = "green is required between 0 and 255")]
        [JsonProperty("green")]
        public int Green { get; set; } = -1;

        [Range(0, 255, ErrorMessage = "blue is required between 0 and 255")]
        [JsonProperty("blue")]
        public int Blue { get; set; } = -1;

        public virtual void Validate()
        {
            
        }
    }
}
