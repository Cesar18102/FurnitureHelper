using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class GlobalPartsConnectionDto
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("comment")]
        public string Comment { get; private set; }

        [JsonProperty("order_number")]
        public int OrderNumber { get; private set; }

        [JsonProperty("sub_connections")]
        public ICollection<TwoPartsConnectionDto> SubConnections { get; private set; }

        [JsonProperty("global_connections_glues")]
        public IEnumerable<ConnectionGlueDto> GlobalConnectionGlues { get; private set; }

        public void SortConnections()
        {
            SubConnections = SubConnections.OrderBy(connection => connection.OrderNumber).ToList();
        }
    }
}