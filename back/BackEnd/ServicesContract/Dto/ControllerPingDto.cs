using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class ControllerPingDto : IDto
    {
        [RegularExpression("^([0-9A-F]{2}:){5}[0-9A-F]{2}$", ErrorMessage = "mac is invalid")]
        [Required(ErrorMessage = "mac is required")]
        [JsonProperty("mac")]
        public string Mac { get; set; }
    }
}
