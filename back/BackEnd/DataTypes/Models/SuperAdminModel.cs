using Newtonsoft.Json;

namespace Models
{
    public class SuperAdminModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("admin")]
        public AdminModel AdminRights { get; private set; }
    }
}
