using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models.Dto
{
    public class PartDto
    {
        public class PartComparer : IEqualityComparer<PartDto>
        {
            public bool Equals(PartDto x, PartDto y) => x.Id == y.Id;
            public int GetHashCode(PartDto obj) => 0;
        }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("model_url")]
        public string ModelUrl { get; private set; }

        [JsonProperty("price")]
        public float? Price { get; private set; }

        [JsonProperty("scale")]
        public float? Scale { get; private set; }

        [JsonProperty("possible_materials")]
        public IEnumerable<MaterialDto> PossibleMaterials { get; private set; }

        [JsonProperty("connection_helpers")]
        public IEnumerable<ConnectionHelperDto> ConnectionHelpers { get; private set; }
    }
}