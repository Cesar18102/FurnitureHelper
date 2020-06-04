using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class DeleteDto : IDto
    {
        [Required(ErrorMessage = "deleted_id is required")]
        [JsonProperty("deleted_id")]
        public int? DeletedId { get; set; }

        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }
    }
}
