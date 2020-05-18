using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class InvariantPartStore : IModel
    {
        [JsonProperty("positions")]
        public ICollection<InvariantPartStorePosition> Positions { get; private set; } = new List<InvariantPartStorePosition>();

        public InvariantPartStore() { }

        public InvariantPartStore(IEnumerable<ConcretePartModel> concreteParts)
        {
            IEnumerable<IGrouping<PartModel, ConcretePartModel>> partGroups = concreteParts.GroupBy(
                part => part.Part, new PartModel.PartComparer()
            );

            foreach (IGrouping<PartModel, ConcretePartModel> partGroup in partGroups)
                Positions.Add(new InvariantPartStorePosition(partGroup.Key, partGroup.Count()));
        }

        public bool Contains(InvariantPartStore store) => Contains(store.Positions);

        public bool Contains(IEnumerable<InvariantPartStorePosition> positions)
        {
            foreach (InvariantPartStorePosition position in positions)
            {
                InvariantPartStorePosition localPosition = Positions.FirstOrDefault(pos => pos.Part.Id == position.Part.Id);

                if (localPosition == null || localPosition.Amount < position.Amount)
                    return false;
            }

            return true;
        }
    }
}
