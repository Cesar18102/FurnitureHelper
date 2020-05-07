using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddManufactirerSell : IDto
    {
        [Required(ErrorMessage = "session is required")]
        [JsonProperty("session")]
        public SessionDto Session { get; set; }

        [MinLength(1, ErrorMessage = "positions list is required")]
        [JsonProperty("positions")]
        public IEnumerable<SellPositionDto> Positions { get; set; }

        public void Validate()
        {
            
        }
    }
}
