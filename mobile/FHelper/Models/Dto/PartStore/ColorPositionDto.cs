using Newtonsoft.Json;

namespace Models.Dto.PartStore
{
    public class ColorPositionDto
    {
        [JsonProperty("color")]
        public PartColorDto Color { get; private set; }

        [JsonProperty("amount")]
        public int Amount { get; private set; }

        public ColorPositionDto() { }

        public ColorPositionDto(PartColorDto color, int count)
        {
            Color = color;
            Amount = count;
        }
    }
}
