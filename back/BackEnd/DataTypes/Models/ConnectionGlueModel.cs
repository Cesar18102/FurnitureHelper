using Newtonsoft.Json;

namespace Models
{
    public class ConnectionGlueModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("comment")]
        public string Comment { get; private set; }

        [JsonProperty("glue_part")]
        public PartModel GluePart { get; private set; }
    }
}