using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class ControllerConfigModel : IModel
    {
        [JsonProperty("indicators")]
        public IEnumerable<int> IndicatorPins { get; private set; }

        [JsonProperty("readers")]
        public IEnumerable<int> ReaderPins { get; private set; }

        public ControllerConfigModel(IEnumerable<int> indicators, IEnumerable<int> readers)
        {
            IndicatorPins = indicators;
            ReaderPins = readers;
        }
    }
}
