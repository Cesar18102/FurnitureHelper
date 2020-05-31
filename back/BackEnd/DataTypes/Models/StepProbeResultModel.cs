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
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("status")]
        public ProbeStatus Status { get; private set; }

        public StepProbeResultModel(string id, ProbeStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}
