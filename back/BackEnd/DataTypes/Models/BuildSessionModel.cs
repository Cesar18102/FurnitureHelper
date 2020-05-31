using Newtonsoft.Json;

namespace Models
{
    public class BuildSessionModel : IModel
    {
        [JsonProperty("build_session_token")]
        public string BuildSessionToken { get; private set; }

        [JsonProperty("furniture_item_id")]
        public int FurnitureItemId { get; private set; }

        public BuildSessionModel(string token, int furnitureItemId)
        {
            BuildSessionToken = token;
            FurnitureItemId = furnitureItemId;
        }
    }
}
