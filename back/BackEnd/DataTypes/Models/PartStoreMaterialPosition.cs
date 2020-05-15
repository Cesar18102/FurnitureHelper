using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartStoreMaterialPosition : IModel
    {
        [JsonProperty("material")]
        public MaterialModel Material { get; private set; }

        [JsonProperty("color_positions")]
        public ICollection<PartStoreMaterialColorPosition> ColorPositions { get; private set; } = new List<PartStoreMaterialColorPosition>();

        [JsonProperty("amount")]
        public int Amount => ColorPositions.Sum(position => position.Amount);

        private PartStoreMaterialPosition() { }

        public static PartStoreMaterialPosition CreateByPossible(MaterialModel material, IEnumerable<ConcretePartModel> concreteParts)
        {
            return new PartStoreMaterialPosition()
            {
                Material = material,
                ColorPositions = material.PossibleColors.Select(color => 
                    new PartStoreMaterialColorPosition(color, concreteParts.Count(cpart => cpart.SelectedColor.Id == color.Id))
                ).ToList()
            };
        }

        public static PartStoreMaterialPosition CreateByConcrete(MaterialModel material, IEnumerable<ConcretePartModel> concreteParts)
        {
            PartStoreMaterialPosition position = new PartStoreMaterialPosition() { Material = material };

            IEnumerable<IGrouping<PartColorModel, ConcretePartModel>> colorGroups = concreteParts.GroupBy(
                part => part.SelectedColor, new PartColorModel.ColorComparer()
            );

            foreach (IGrouping<PartColorModel, ConcretePartModel> colorGroup in colorGroups)
                position.ColorPositions.Add(new PartStoreMaterialColorPosition(colorGroup.Key, colorGroup.Count()));

            return position;
        }
    }
}
