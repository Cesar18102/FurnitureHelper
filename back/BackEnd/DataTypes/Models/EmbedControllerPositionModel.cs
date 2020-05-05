using Newtonsoft.Json;

namespace Models
{
    public class EmbedControllerPositionModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("pos_x")]
        public float PosX { get; private set; }

        [JsonProperty("pos_y")]
        public float PosY { get; private set; }

        [JsonProperty("pos_z")]
        public float PosZ { get; private set; }   
    }
}