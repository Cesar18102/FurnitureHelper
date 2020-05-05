using Newtonsoft.Json;

namespace Models
{
    public class PartColorModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("hex")]
        public string Hex { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }
    }
}