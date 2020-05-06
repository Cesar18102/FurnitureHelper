using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddConcretePartDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(ErrorMessage = "part_id is required")]
        [JsonProperty("part_id")]
        public int? PartId { get; set; }

        [Required(ErrorMessage = "material_id is required")]
        [JsonProperty("material_id")]
        public int? MaterialId { get; set; }

        [Required(ErrorMessage = "color_id is required")]
        [JsonProperty("color_id")]
        public int? ColorId { get; set; }

        [JsonProperty("embedded_controllers")]
        public IEnumerable<ConcreteControllerDto> EmbeddedControllers { get; set; }

        public void Validate()
        {
            
        }
    }
}
