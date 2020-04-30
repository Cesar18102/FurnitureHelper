using System.Collections.Generic;

namespace BackEnd.Models
{
    public class FurnitureItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<GlobalPartsConnection> GlobalConnections { get; private set; }
    }
}