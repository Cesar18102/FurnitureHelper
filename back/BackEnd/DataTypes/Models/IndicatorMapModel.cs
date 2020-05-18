using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class IndicatorMapModel : IModel
    {
        [JsonProperty("enabled_indicator_pins")]
        public ICollection<int> EnabledIndicatorPins { get; private set; } = new List<int>();
    }
}
