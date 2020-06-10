using System.Collections.Generic;

using Models.Dto;

using Newtonsoft.Json;

namespace Models
{
    public class TwoPartsConnectionDto
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("comment")]
        public string Comment { get; private set; }

        [JsonProperty("order_number")]
        public int OrderNumber { get; private set; }


        [JsonProperty("connection_helper")]
        public ConnectionHelperDto ConnectionHelper { get; private set; }

        [JsonProperty("part")]
        public PartDto Part { get; private set; }

        [JsonProperty("used_part_id")]
        public int UsedPartId { get; private set; }


        [JsonProperty("connection_helper_other")]
        public ConnectionHelperDto ConnectionHelperOther { get; private set; }

        [JsonProperty("part_other")]
        public PartDto PartOther { get; private set; }

        [JsonProperty("used_part_other_id")]
        public int UsedPartOtherId { get; private set; }


        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueDto> ConnectionGlues { get; private set; }
    }
}