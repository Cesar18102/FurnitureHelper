using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class StartBuildDto : IDto
    {
        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [Required(ErrorMessage = "furniture_id is required")]
        [JsonProperty("furniture_id")]
        public int? FurnitureId { get; set; }
    }
}
