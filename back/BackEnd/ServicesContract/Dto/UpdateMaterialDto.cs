using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateMaterialDto : IDto
    {
        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price_coeff")]
        public float? PriceCoefficient { get; set; }

        [Url(ErrorMessage = "texture_url must be url")]
        [JsonProperty("texture_url")]
        public string TextureUrl { get; set; }

        [JsonProperty("possible_colors")]
        public IEnumerable<int> PossibleColors { get; set; }
    }
}
