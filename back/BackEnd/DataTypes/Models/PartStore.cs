using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartStore : IModel
    {
        [JsonProperty("positions")]
        public ICollection<PartStorePosition> Positions { get; private set; } = new List<PartStorePosition>();
    }
}
