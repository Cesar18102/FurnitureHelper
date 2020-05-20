using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto
{
    public class MaterialDto
    {
        public class MaterialComparer : IEqualityComparer<MaterialDto>
        {
            public bool Equals(MaterialDto x, MaterialDto y) => x.Id == y.Id;
            public int GetHashCode(MaterialDto obj) => 0;
        }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("price_coeff")]
        public float? PriceCoefficient { get; private set; }

        [JsonProperty("texture_url")]
        public string TextureUrl { get; private set; }

        [JsonProperty("possible_colors")]
        public IEnumerable<PartColorDto> PossibleColors { get; private set; }
    }
}