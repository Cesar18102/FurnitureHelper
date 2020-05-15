using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartStore : IModel
    {
        [JsonProperty("positions")]
        public ICollection<PartStorePosition> Positions { get; private set; } = new List<PartStorePosition>();

        private PartStore() { }

        public PartStore(IEnumerable<PartModel> parts, IEnumerable<ConcretePartModel> concreteParts)
        {
            Positions = parts.Select(part => 
                PartStorePosition.CreateByPossible(part, concreteParts.Where(cpart => cpart.Part.Id == part.Id))
            ).ToList();
        }

        public PartStore(IEnumerable<ConcretePartModel> concreteParts)
        {
            IEnumerable<IGrouping<PartModel, ConcretePartModel>> partGroups = concreteParts.GroupBy(
                part => part.Part, new PartModel.PartComparer()
            );

            foreach (IGrouping<PartModel, ConcretePartModel> partGroup in partGroups)
                Positions.Add(PartStorePosition.CreateByConcrete(partGroup.Key, partGroup.ToList()));
        }
    }
}
