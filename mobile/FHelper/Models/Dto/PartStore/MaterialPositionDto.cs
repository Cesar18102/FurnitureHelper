using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto.PartStore
{
    public class MaterialPositionDto
    {
        [JsonProperty("material")]
        public MaterialDto Material { get; private set; }

        [JsonProperty("color_positions")]
        public ICollection<ColorPositionDto> ColorPositions { get; private set; } = new List<ColorPositionDto>();

        [JsonProperty("amount")]
        public int Amount => ColorPositions.Sum(position => position.Amount);
    }
}
