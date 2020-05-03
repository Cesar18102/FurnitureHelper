using System.Collections.Generic;

namespace Models
{
    public class MaterialModel : IModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float PriceCoefficient { get; private set; }
        public string TextureUrl { get; private set; }
        public IEnumerable<PartColorModel> PossibleColors { get; private set; }
    }
}