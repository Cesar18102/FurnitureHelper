using System.ComponentModel.DataAnnotations;

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

        [RegularExpression("^([0-9A-F]{2}:){5}[0-9A-F]{2}$", ErrorMessage = "mac is invalid")]
        [JsonProperty("controller_mac")]
        public string ControllerMac { get; set; }

        [JsonProperty("amount")]
        public int? Amount { get; set; }
    }
}
