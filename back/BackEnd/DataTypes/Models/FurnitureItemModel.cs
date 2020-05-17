﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class FurnitureItemModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("scale")]
        public float? Scale { get; private set; }

        [JsonProperty("used_parts")]
        public ICollection<UsedPartModel> UsedParts { get; private set; }

        [JsonProperty("global_connections")]
        public IEnumerable<GlobalPartsConnectionModel> GlobalConnections { get; private set; }   
    }
}