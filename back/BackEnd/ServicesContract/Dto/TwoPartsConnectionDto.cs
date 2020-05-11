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

        [Required(ErrorMessage = "connection_helper_id is required")]
        [JsonProperty("connection_helper_id")]
        public int? ConnectionHelperId { get; set; }

        [Required(ErrorMessage = "connection_helper_other_id is required")]
        [JsonProperty("connection_helper_other_id")]
        public int? ConnectionHelperOtherId { get; set; }

        [Required(ErrorMessage = "used_part_id is required")]
        [JsonProperty("used_part_id")]
        public int? UsedPartId { get; private set; }

        [Required(ErrorMessage = "used_part_other_id is required")]
        [JsonProperty("used_part_other_id")]
        public int? UsedPartOtherId { get; private set; }

        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueDto> ConnectionGlues { get; set; }
    }
}
