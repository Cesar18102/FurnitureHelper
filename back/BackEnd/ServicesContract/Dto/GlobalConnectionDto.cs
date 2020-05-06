using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class GlobalConnectionDto : IDto
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "order_number is required")]
        [JsonProperty("order_number")]
        public int? OrderNumber { get; set; }

        [JsonProperty("sub_connections")]
        public IEnumerable<TwoPartsConnectionDto> SubConnections { get; set; }

        [JsonProperty("global_connection_glues")]
        public IEnumerable<ConnectionGlueDto> GlobalConnectionGlues { get; set; }

        public void Validate() { }
    }
}
