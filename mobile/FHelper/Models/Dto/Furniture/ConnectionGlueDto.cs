using Models.Dto;

using Newtonsoft.Json;

namespace Models
{
    public class ConnectionGlueDto
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("comment")]
        public string Comment { get; private set; }

        [JsonProperty("glue_part")]
        public PartDto GluePart { get; private set; }
    }
}