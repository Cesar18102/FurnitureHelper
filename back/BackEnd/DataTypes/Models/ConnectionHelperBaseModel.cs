using Newtonsoft.Json;

namespace Models
{
    public class ConnectionHelperModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("pos_x")]
        public float PosX { get; private set; }

        [JsonProperty("pos_y")]
        public float PosY { get; private set; }

        [JsonProperty("pos_z")]
        public float PosZ { get; private set; }

        [JsonProperty("pos_x_other")]
        public float PosXOther { get; private set; }

        [JsonProperty("pos_y_other")]
        public float PosYOther { get; private set; }

        [JsonProperty("pos_z_other")]
        public float PosZOther { get; private set; }

        [JsonProperty("indicator_pin_number")]
        public int IndicatorPinNumber { get; private set; }

        [JsonProperty("reader_pin_number")]
        public int ReaderPinNumber { get; private set; }

        [JsonProperty("reader_pin_number_other")]
        public int ReaderPinNumberOther { get; set; }
    }
}