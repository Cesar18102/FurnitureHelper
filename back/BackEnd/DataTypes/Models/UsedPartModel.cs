using Newtonsoft.Json;

namespace Models
{
    public class UsedPartModel : IModel
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("part_id")]
        public int PartId { get; private set; }

        [JsonProperty("furniture_item_id")]
        public int FurnitureItemId { get; private set; }

        public UsedPartModel() { }

        public UsedPartModel(int partId)
        {
            PartId = partId;
        }
    }
}
