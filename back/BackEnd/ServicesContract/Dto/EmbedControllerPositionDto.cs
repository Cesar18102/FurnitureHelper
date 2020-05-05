using System;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class EmbedControllerPositionDto : IDto
    {
        [Required(ErrorMessage = "pos_x is required")]
        [JsonProperty("pos_x")]
        public float? PosX { get; private set; }

        [Required(ErrorMessage = "pos_y is required")]
        [JsonProperty("pos_y")]
        public float? PosY { get; private set; }

        [Required(ErrorMessage = "pos_z is required")]
        [JsonProperty("pos_z")]
        public float? PosZ { get; private set; }

        public void Validate()
        {
            
        }
    }
}
