﻿using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace ServicesContract.Dto
{
    public class LogInDto : IDto
    {
        [Required(ErrorMessage = "login is required")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "password_salted is required")]
        [JsonProperty("password_salted")]
        public string PasswordSalted { get; set; }

        [Required(ErrorMessage = "salt is required")]
        [JsonProperty("salt")]
        public string Salt { get; set; }
    }
}
