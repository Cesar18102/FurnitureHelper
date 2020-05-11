using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddAdminDto : IDto
    {
        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

        [Required(ErrorMessage = "account_id is required")]
        [JsonProperty("account_id")]
        public int? AccountId { get; set; }
    }
}
