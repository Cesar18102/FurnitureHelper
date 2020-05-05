using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddMaterialDto : IDto
    {
        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "price_coeff is required")]
        [JsonProperty("price_coeff")]
        public float PriceCoefficient { get; set; } = -1;

        [Required(AllowEmptyStrings = false, ErrorMessage = "texture_url is required")]
        [Url(ErrorMessage = "texture_url must be url")]
        [JsonProperty("texture_url")]
        public string TextureUrl { get; set; }

        [JsonProperty("possible_colors")]
        public IEnumerable<int> PossibleColors { get; set; }

        public void Validate()
        {
            
        }
    }
}
