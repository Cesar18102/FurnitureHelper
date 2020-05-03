using System.Collections.Generic;

using Newtonsoft.Json;

namespace DataTypes.Models
{
    public class AccountModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("login")]
        public string Login { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonIgnore]
        public string Password { get; private set; }

        [JsonProperty("first_name")]
        public string FirstName { get; private set; }

        [JsonProperty("last_name")]
        public string LastName { get; private set; }

        [JsonProperty("account_extensions")]
        public IEnumerable<AccountExtensionModel> AccountExtensions { get; private set; }
    }
}