using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class ConnectionGlueDto : IDto
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "glue_part_id is required")]
        [JsonProperty("glue_part_id")]
        public int? GluePartId { get; set; }

        [Required(ErrorMessage = "pos_x is required")]
        [JsonProperty("pos_x")]
        public float? PosX { get; set; }

        [Required(ErrorMessage = "pos_y is required")]
        [JsonProperty("pos_y")]
        public float? PosY { get; set; }

        [Required(ErrorMessage = "pos_z is required")]
        [JsonProperty("pos_z")]
        public float? PosZ { get; set; }

        public void Validate()
        {
        }
    }
}
