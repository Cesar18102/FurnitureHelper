﻿using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class SellPositionDto : IDto
    {
        [Required(ErrorMessage = "part_id is required")]
        [JsonProperty("part_id")]
        public int? PartId { get; set; }

        [Required(ErrorMessage = "material_id is required")]
        [JsonProperty("material_id")]
        public int? MaterialId { get; set; }

        [Required(ErrorMessage = "color_id is required")]
        [JsonProperty("color_id")]
        public int? ColorId { get; set; }

        [Required(ErrorMessage = "count is required")]
        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}
