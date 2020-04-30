using System.Collections.Generic;

namespace BackEnd.Models
{
    public class Material
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float PriceCoefficient { get; private set; }
        public string TextureUrl { get; private set; }
        public IEnumerable<PartColor> PossibleColors { get; private set; }
    }
}