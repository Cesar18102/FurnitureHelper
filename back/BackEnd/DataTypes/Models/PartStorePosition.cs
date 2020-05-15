using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartStorePosition : IModel
    {
        [JsonProperty("part")]
        public PartModel Part { get; private set; }

        [JsonProperty("material_positions")]
        public ICollection<PartStoreMaterialPosition> MaterialPositions { get; private set; } = new List<PartStoreMaterialPosition>();

        [JsonProperty("amount")]
        public int Amount => MaterialPositions.Sum(position => position.Amount);

        private PartStorePosition() { }

        public static PartStorePosition CreateByPossible(PartModel part, IEnumerable<ConcretePartModel> concreteParts)
        {
            return new PartStorePosition()
            {
                Part = part,
                MaterialPositions = part.PossibleMaterials.Select(material => 
                    PartStoreMaterialPosition.CreateByPossible(
                        material, concreteParts.Where(cpart => cpart.SelectedMaterial.Id == material.Id)
                    )
                ).ToList()
            };
        }

        public static PartStorePosition CreateByConcrete(PartModel part, IEnumerable<ConcretePartModel> concreteParts)
        {
            PartStorePosition position = new PartStorePosition() { Part = part };

            IEnumerable<IGrouping<MaterialModel, ConcretePartModel>> materialGroups = concreteParts.GroupBy(
                cpart => cpart.SelectedMaterial, new MaterialModel.MaterialComparer()
            );

            foreach (IGrouping<MaterialModel, ConcretePartModel> materialGroup in materialGroups)
                position.MaterialPositions.Add(PartStoreMaterialPosition.CreateByConcrete(materialGroup.Key, materialGroup.ToList()));

            return position;
        }
    }
}
