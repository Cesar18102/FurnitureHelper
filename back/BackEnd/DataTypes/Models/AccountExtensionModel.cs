using System;

using Newtonsoft.Json;

namespace Models
{
    public class AccountExtensionModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("account_id")]
        public int AccountId { get; private set; }

        [JsonProperty("phone")]
        public string Phone { get; private set; }

        [JsonProperty("address")]
        public string Address { get; private set; }

        [JsonProperty("last_used_date")]
        public DateTime LastUsedDate { get; set; } = DateTime.Now;
    }
}