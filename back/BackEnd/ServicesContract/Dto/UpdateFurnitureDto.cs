using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateFurnitureDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("used_parts_to_remove")]
        public IEnumerable<int> UsedPartsToRemove { get; set; } = new List<int>();

        [JsonProperty("used_parts_to_add")]
        public IEnumerable<UsedPartsDto> UsedPartsToAdd { get; set; } = new List<UsedPartsDto>();

        public void Validate() { }
    }
}
