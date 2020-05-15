using Newtonsoft.Json;

namespace Models
{
    public class PartStoreMaterialColorPosition : IModel
    {
        [JsonProperty("color")]
        public PartColorModel Color { get; private set; }

        [JsonProperty("amount")]
        public int Amount { get; private set; }

        public PartStoreMaterialColorPosition(PartColorModel color, int count)
        {
            Color = color;
            Amount = count;
        }
    }
}
