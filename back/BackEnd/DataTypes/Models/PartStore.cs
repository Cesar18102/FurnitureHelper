using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartStore : IModel
    {
        [JsonProperty("positions")]
        public ICollection<PartStorePosition> Positions { get; private set; } = new List<PartStorePosition>();

        public PartStore() { }

        public PartStore(IEnumerable<ConcretePartModel> concreteParts)
        {
            foreach (ConcretePartModel concretePart in concreteParts)
            {
                PartStorePosition storePosition = Positions.FirstOrDefault(position => position.Part.Id == concretePart.Part.Id);
                if (storePosition == null)
                    Positions.Add(new PartStorePosition(concretePart.Part, 1));
                else
                    storePosition.Increase();
            }
        }

        public bool Contains(PartStore store) => Contains(store.Positions);

        public bool Contains(IEnumerable<PartStorePosition> positions)
        {
            foreach (PartStorePosition position in positions)
            {
                PartStorePosition localPosition = positions.FirstOrDefault(pos => pos.Part.Id == position.Part.Id);

                if (localPosition == null || localPosition.Amount < position.Amount)
                    return false;
            }

            return true;
        }

        public void Filter(Predicate<PartStorePosition> predicate)
        {
            Positions = Positions.Where(position => predicate(position)).ToList();
        }
    }
}
