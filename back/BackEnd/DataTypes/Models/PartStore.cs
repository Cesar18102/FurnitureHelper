using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Models
{
    public class PartStore : IModel
    {
        [JsonProperty("positions")]
        public ICollection<PartStorePosition> Positions { get; private set; } = new List<PartStorePosition>();

        public bool Contains(PartStore store)
        {
            foreach (PartStorePosition position in store.Positions)
            {
                PartStorePosition localPosition = Positions.FirstOrDefault(pos => pos.Part.Id == position.Part.Id);

                if (localPosition == null || localPosition.Amount < position.Amount)
                    return false;
            }

            return true;
        }
    }
}
