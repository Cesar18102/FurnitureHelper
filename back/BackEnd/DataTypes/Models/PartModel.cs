using System.Collections.Generic;

namespace Models
{
    public class PartModel : IModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ModelUrl { get; private set; }   
        public float Price { get; private set; }
        public IEnumerable<MaterialModel> PossibleMaterials { get; private set; }
    }
}