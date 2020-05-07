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

        [JsonProperty("nested_global_connection_order_number")]
        public int? NestedGlobalConnectionOrderNumber { get; set; }

        [JsonProperty("nested_two_parts_connection_order_number")]
        public int? NestedTwoPartsConnectionOrderNumber { get; set; }

        [JsonProperty("connect_to_first_if_equal")]
        public bool? ConnectToFirstIfEqual { get; set; }

        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueDto> ConnectionGlues { get; set; }

        public void Validate()
        {
            
        }
    }
}
