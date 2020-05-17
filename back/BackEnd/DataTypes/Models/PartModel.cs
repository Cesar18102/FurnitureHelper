using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartModel : IModel
    {
        public class PartComparer : IEqualityComparer<PartModel>
        {
            public bool Equals(PartModel x, PartModel y) => x.Id == y.Id;
            public int GetHashCode(PartModel obj) => 0;
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

        [JsonProperty("in_furniture_scale")]
        public float? InFurnitureScale { get; private set; }

        [JsonProperty("possible_materials")]
        public IEnumerable<MaterialModel> PossibleMaterials { get; private set; }

        [JsonProperty("connection_helpers")]
        public IEnumerable<ConnectionHelperModel> ConnectionHelpers { get; private set; }
    }
}