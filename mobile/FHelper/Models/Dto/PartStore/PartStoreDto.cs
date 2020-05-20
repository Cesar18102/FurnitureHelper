using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto.PartStore
{
    public class PartStoreDto
    {
        [JsonProperty("positions")]
        public ICollection<PartPositionDto> Positions { get; private set; } = new List<PartPositionDto>();
    }
}
