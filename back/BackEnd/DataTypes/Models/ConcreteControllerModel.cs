using Newtonsoft.Json;

namespace Models
{
    public class ConcreteControllerModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("mac")]
        public string MAC { get; private set; }

        [JsonProperty("embed_position")]
        public EmbedControllerPositionModel EmbedPosition { get; private set; }
    }
}