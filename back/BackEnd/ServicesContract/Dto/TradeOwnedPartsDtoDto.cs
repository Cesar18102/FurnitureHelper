using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class TradeOwnedPartsDto : IDto
    {
        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [MinLength(1, ErrorMessage = "positions list is required")]
        [JsonProperty("positions")]
        public SellPositionDto[] Positions { get; set; }
    }
}
