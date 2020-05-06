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

        [JsonProperty("embed_controller_position")]
        public EmbedControllerPositionModel ControllerPosition { get; private set; }

        [JsonProperty("embed_controller_position_other")]
        public EmbedControllerPositionModel ControllerPositionOther { get; private set; }

        [JsonProperty("connection_glues")]
        public IEnumerable<ConnectionGlueModel> ConnectionGlues { get; private set; }   
    }
}