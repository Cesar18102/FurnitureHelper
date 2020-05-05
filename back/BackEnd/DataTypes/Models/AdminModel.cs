using Newtonsoft.Json;

namespace Models
{
    public class AdminModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("account")]
        public AccountModel Account { get; private set; }
    }
}
