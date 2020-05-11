using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class ConnectionsDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(ErrorMessage = "furniture_item_id is required")]
        [JsonProperty("furniture_item_id")]
        public int? FurnitureItemId { get; set; }

        [JsonProperty("global_connections")]
        public IEnumerable<GlobalConnectionDto> GlobalConnections { get; set; }
    }
}
