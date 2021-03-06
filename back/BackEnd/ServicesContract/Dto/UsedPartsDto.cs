﻿using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UsedPartsDto : IDto
    {
        [Required(ErrorMessage = "part_id is required")]
        [JsonProperty("part_id")]
        public int? PartId { get; set; }

        [Required(ErrorMessage = "count is required")]
        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}
