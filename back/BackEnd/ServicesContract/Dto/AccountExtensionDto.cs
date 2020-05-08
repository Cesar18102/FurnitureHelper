using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AccountExtensionDto : IDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "phone is required")]
        [Phone(ErrorMessage = "phone is invalid")]
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "address is required")]
        [JsonProperty("address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "account_id is required")]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }

        public void Validate()
        {

        }
    }
}
