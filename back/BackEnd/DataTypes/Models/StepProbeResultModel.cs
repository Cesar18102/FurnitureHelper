using Newtonsoft.Json;

namespace Models
{
    public enum ProbeStatus
    {
        DONE,
        ERROR,
        PENDING,
        FINISHED
    };

    public class StepProbeResultModel : IModel
    {
        [JsonProperty("status")]
        public ProbeStatus Status { get; private set; }

        public StepProbeResultModel(ProbeStatus status)
        {
            Status = status;
        }
    }
}
