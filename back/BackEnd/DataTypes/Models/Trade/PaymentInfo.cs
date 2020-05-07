using Newtonsoft.Json;

namespace Models.Trade
{
    public class PaymentInfo : IModel
    {
        [JsonProperty("data")]
        public string Data { get; private set; }

        [JsonProperty("signature")]
        public string Signature { get; private set; }

        public PaymentInfo(string data, string signature)
        {
            Data = data;
            Signature = signature;
        }
    }
}
