﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class UpdateFurnitureDto : IDto
    {
        [Required(ErrorMessage = "admin_session is required")]
        [JsonProperty("admin_session")]
        public SessionDto AdminSession { get; set; }

        [Required(ErrorMessage = "id is required")]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("global_connections")]
        public IEnumerable<GlobalConnectionDto> GlobalConnections { get; set; }

        public void Validate() { }
    }
}