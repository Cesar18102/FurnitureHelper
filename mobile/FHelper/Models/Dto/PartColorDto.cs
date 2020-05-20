using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto
{
    public class PartColorDto
    {
        public class ColorComparer : IEqualityComparer<PartColorDto>
        {
            public bool Equals(PartColorDto x, PartColorDto y) => x.Id == y.Id;
            public int GetHashCode(PartColorDto obj) => 0;
        }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("hex")]
        public string Hex { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }
    }
}