using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddFurnitureDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("used_parts")]
        public IEnumerable<UsedPartsDto> UsedParts { get; set; } = new List<UsedPartsDto>();
    }
}
