using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class FurnitureItemDto
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("scale")]
        public float? Scale { get; private set; }

        [JsonProperty("model_url")]
        public string ModelUrl { get; private set; }

        [JsonProperty("used_parts")]
        public ICollection<UsedPartDto> UsedParts { get; private set; }

        [JsonProperty("global_connections")]
        public ICollection<GlobalPartsConnectionDto> GlobalConnections { get; private set; }   

        public void SortConnections()
        {
            GlobalConnections = GlobalConnections.OrderBy(connection => connection.OrderNumber).ToList();
            foreach (GlobalPartsConnectionDto connection in GlobalConnections)
                connection.SortConnections();
        }
    }
}