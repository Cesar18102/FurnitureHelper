using Newtonsoft.Json;

namespace Models
{
    public class InvariantPartStorePosition : IModel
    {
        [JsonProperty("part")]
        public PartModel Part { get; private set; }

        [JsonProperty("amount")]
        public int Amount { get; private set; }

        public InvariantPartStorePosition(PartModel part, int amount)
        {
            Part = part;
            Amount = amount;
        }
    }
}
