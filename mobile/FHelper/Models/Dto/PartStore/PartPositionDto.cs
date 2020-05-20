using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto.PartStore
{
    public class PartPositionDto
    {
        [JsonProperty("part")]
        public PartDto Part { get; private set; }

        [JsonProperty("material_positions")]
        public ICollection<MaterialPositionDto> MaterialPositions { get; private set; } = new List<MaterialPositionDto>();

        [JsonProperty("amount")]
        public int Amount => MaterialPositions.Sum(position => position.Amount);

        private PartPositionDto() { }
    }
}
