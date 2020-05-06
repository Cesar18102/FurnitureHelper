using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class TwoPartsConnectionDto : IDto
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "order_number is required")]
        [JsonProperty("order_number")]
        public int? OrderNumber { get; set; }

        [Required(ErrorMessage = "embed_controller_position_id is required")]
        [JsonProperty("embed_controller_position_id")]
        public int? EmbedControllerPositionId { get; set; }

        [Required(ErrorMessage = "embed_controller_position_other_id is required")]
        [JsonProperty("embed_controller_position_other_id")]
        public int? EmbedControllerPositionOtherId { get; set; }

        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueDto> ConnectionGlues { get; set; }

        public void Validate()
        {
            
        }
    }
}
