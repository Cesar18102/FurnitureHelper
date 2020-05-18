using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class StepProbeDto : IDto
    {
        [Required(ErrorMessage = "mac is required")]
        [RegularExpression("^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$", ErrorMessage = "mac is invalid")]
        [JsonProperty("mac")]
        public string Mac { get; set; }

        [JsonProperty("active_readers")]
        public IEnumerable<int> ActiveReaders { get; set; } //change to two readers or remove at all
    }
}
