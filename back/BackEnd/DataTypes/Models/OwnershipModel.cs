using Newtonsoft.Json;

namespace Models
{
    public class OwnershipModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonIgnore]
        public int AccountId { get; private set; }

        [JsonProperty("concrete_part")]
        public ConcretePartModel ConcretePart { get; private set; }

        public OwnershipModel() { }

        public OwnershipModel(int accountId, ConcretePartModel concretePart)
        {
            AccountId = accountId;
            ConcretePart = concretePart;
        }
    }
}
