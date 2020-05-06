using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class ConcreteControllerDto : IDto
    {
        [Required(ErrorMessage = "mac is required")]
        [RegularExpression("^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$", ErrorMessage = "mac is invalid")]
        [JsonProperty("mac")]
        public string MAC { get; set; }

        [Required(ErrorMessage = "embed_position_id is required")]
        [JsonProperty("embed_position_id")]
        public int? EmbedPositionId { get; set; }

        public void Validate()
        {
            
        }
    }
}
