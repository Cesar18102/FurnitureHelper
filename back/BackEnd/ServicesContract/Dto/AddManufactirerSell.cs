using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddManufacturerSellDto : IDto
    {
        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [MinLength(1, ErrorMessage = "positions list is required")]
        [JsonProperty("positions")]
        public SellPositionDto[] Positions { get; set; }

        [JsonProperty("existing_account_extension_id")]
        public int? ExisitingAccountExtensionId { get; set; }

        [JsonProperty("new_account_extension")]
        public AccountExtensionDto NewAccountExtension { get; set; }

        public void Validate()
        {
            
        }
    }
}
