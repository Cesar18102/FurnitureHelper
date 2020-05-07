﻿using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class ConnectionHelperDto : IDto
    {
        [Required(ErrorMessage = "pos_x is required")]
        [JsonProperty("pos_x")]
        public float? PosX { get; set; }

        [Required(ErrorMessage = "pos_y is required")]
        [JsonProperty("pos_y")]
        public float? PosY { get; set; }

        [Required(ErrorMessage = "pos_z is required")]
        [JsonProperty("pos_z")]
        public float? PosZ { get; set; }

        [Required(ErrorMessage = "indicator_pin_number is required")]
        [JsonProperty("indicator_pin_number")]
        public int? IndicatorPinNumber { get; set; }

        [Required(ErrorMessage = "reader_pin_number is required")]
        [JsonProperty("reader_pin_number")]
        public int? ReaderPinNumber { get; set; }

        public void Validate()
        {
            
        }
    }
}