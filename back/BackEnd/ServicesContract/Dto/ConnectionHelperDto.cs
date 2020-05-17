using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "pos_x_other is required")]
        [JsonProperty("pos_x_other")]
        public float? PosXOther { get; set; }

        [Required(ErrorMessage = "pos_y_other is required")]
        [JsonProperty("pos_y_other")]
        public float? PosYOther { get; set; }

        [Required(ErrorMessage = "pos_z_other is required")]
        [JsonProperty("pos_z_other")]
        public float? PosZOther { get; set; }

        [Required(ErrorMessage = "pos_x_help is required")]
        [JsonProperty("pos_x_help")]
        public float PosXHelp { get; set; }

        [Required(ErrorMessage = "pos_y_help is required")]
        [JsonProperty("pos_y_help")]
        public float PosYHelp { get; set; }

        [Required(ErrorMessage = "pos_z_help is required")]
        [JsonProperty("pos_z_help")]
        public float PosZHelp { get; set; }

        [Required(ErrorMessage = "indicator_pin_number is required")]
        [JsonProperty("indicator_pin_number")]
        public int? IndicatorPinNumber { get; set; }

        [Required(ErrorMessage = "reader_pin_number is required")]
        [JsonProperty("reader_pin_number")]
        public int? ReaderPinNumber { get; set; }

        [Required(ErrorMessage = "reader_pin_number_other is required")]
        [JsonProperty("reader_pin_number_other")]
        public int? ReaderPinNumberOther { get; set; }
    }
}
