using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class MaterialModel : IModel
    {
        public class MaterialComparer : IEqualityComparer<MaterialModel>
        {
            public bool Equals(MaterialModel x, MaterialModel y) => x.Id == y.Id;
            public int GetHashCode(MaterialModel obj) => 0;
        }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("price_coeff")]
        public float PriceCoefficient { get; private set; }

        [JsonProperty("texture_url")]
        public string TextureUrl { get; private set; }

        [JsonProperty("possible_colors")]
        public IEnumerable<PartColorModel> PossibleColors { get; private set; }
    }
}