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

        [JsonProperty("connection_helper_other")]
        public ConnectionHelperModel ConnectionHelperOther { get; private set; }

        [JsonProperty("part_other")]
        public PartModel PartOther { get; private set; }

        [JsonProperty("nested_global_connection_order_number")]
        public int? NestedGlobalConnectionOrderNumber { get; private set; }

        [JsonProperty("nested_two_parts_connection_order_number")]
        public int? NestedTwoPartsConnectionOrderNumber { get; private set; }

        [JsonProperty("connect_to_first_if_equal")]
        public bool? ConnectToFirstIfEqual { get; private set; }

        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueModel> ConnectionGlues { get; private set; }   
    }
}