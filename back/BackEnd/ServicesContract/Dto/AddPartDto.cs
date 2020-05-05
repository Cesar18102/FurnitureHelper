﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class AddPartDto : IDto
    {
        [Required(ErrorMessage = "super_admin_session is required")]
        [JsonProperty("super_admin_session")]
        public SessionDto SuperAdminSession { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "model_url is required")]
        [Url(ErrorMessage = "model_url must be url")]
        [JsonProperty("model_url")]
        public string ModelUrl { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "price is required")]
        [JsonProperty("price")]
        public float Price { get; set; } = -1;

        [JsonProperty("possible_materials")]
        public IEnumerable<int> PossibleMaterials { get; set; }

        [JsonProperty("embed_controllers_positions")]
        public IEnumerable<EmbedControllerPositionDto> EmbedControllersPositions { get; set; }

        public void Validate()
        {
            
        }
    }
}