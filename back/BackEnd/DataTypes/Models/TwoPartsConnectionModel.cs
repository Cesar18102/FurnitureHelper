using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class TwoPartsConnectionModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("comment")]
        public string Comment { get; private set; }

        [JsonProperty("order_number")]
        public int OrderNumber { get; private set; }


        [JsonProperty("connection_helper")]
        public ConnectionHelperModel ConnectionHelper { get; private set; }

        [JsonProperty("part")]
        public PartModel Part { get; private set; }

        [JsonProperty("used_part_id")]
        public int UsedPartId { get; private set; }


        [JsonProperty("connection_helper_other")]
        public ConnectionHelperModel ConnectionHelperOther { get; private set; }

        [JsonProperty("part_other")]
        public PartModel PartOther { get; private set; }

        [JsonProperty("used_part_other_id")]
        public int UsedPartOtherId { get; private set; }


        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueModel> ConnectionGlues { get; private set; }
    }
}