using Newtonsoft.Json;

namespace Models
{
    public class PartStorePosition : IModel
    {
        [JsonProperty("part")]
        public PartModel Part { get; private set; }

        [JsonProperty("amount")]
        public int Amount { get; private set; }

        public void Increase() => ++Amount;

        public PartStorePosition(PartModel part, int amount)
        {
            Part = part;
            Amount = amount;
        }
    }
}
