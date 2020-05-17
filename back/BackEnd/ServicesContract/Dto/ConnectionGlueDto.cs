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
    }
}
