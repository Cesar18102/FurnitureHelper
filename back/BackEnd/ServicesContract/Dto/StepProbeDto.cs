using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public enum StateChange
    {
        DETACHED = -1,
        UNCHANGED = 0,
        ATTACHED = 1
    };

    public class PinStateChange
    {
        [JsonProperty("pin")]
        public int PinNumber { get; private set; }

        [JsonProperty("change")]
        public StateChange Change { get; private set; }
    }

    public class StepProbeDto : IDto
    {
        [Required(ErrorMessage = "mac is required")]
        [RegularExpression("^([0-9A-F]{2}:){5}[0-9A-F]{2}$", ErrorMessage = "mac is invalid")]
        [JsonProperty("mac")]
        public string Mac { get; set; }

        [JsonProperty("pin_state_changes")]
        public PinStateChange[] PinStateChanges { get; set; } 
    }
}
