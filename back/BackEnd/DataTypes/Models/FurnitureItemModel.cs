using System.Collections.Generic;

namespace DataTypes.Models
{
    public class FurnitureItemModel : IModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<GlobalPartsConnectionModel> GlobalConnections { get; private set; }

        
    }
}