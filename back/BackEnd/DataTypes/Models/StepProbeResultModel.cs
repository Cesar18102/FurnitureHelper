using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class StepProbeResultModel : IModel
    {
        [JsonProperty("wrong_pins")]
        public IEnumerable<int> WrongPins { get; private set; } = new List<int>();

        public StepProbeResultModel(IEnumerable<int> wrongPins)
        {
            WrongPins = wrongPins.ToList();
        }
    }
}
