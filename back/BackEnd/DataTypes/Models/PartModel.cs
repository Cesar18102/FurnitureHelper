using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class PartModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("model_url")]
        public string ModelUrl { get; private set; }

        [JsonProperty("price")]
        public float Price { get; private set; }

        [JsonProperty("possible_materials")]
        public IEnumerable<MaterialModel> PossibleMaterials { get; private set; }

        [JsonProperty("embedded_controllers_positions")]
        public IEnumerable<EmbedControllerPositionModel> EmbedControllersPositions { get; private set; }
    }
}