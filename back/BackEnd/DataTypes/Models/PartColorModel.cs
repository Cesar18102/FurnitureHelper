using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartColorModel : IModel
    {
        public class ColorComparer : IEqualityComparer<PartColorModel>
        {
            public bool Equals(PartColorModel x, PartColorModel y) => x.Id == y.Id;
            public int GetHashCode(PartColorModel obj) => 0;
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