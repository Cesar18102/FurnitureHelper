using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Models
{
    public class ConcretePartModel : IModel
    {
        [JsonProperty("material")]
        public MaterialModel SelectedMaterial { get; private set; }

        [JsonProperty("color")]
        public PartColorModel SelectedColor { get; private set; }

        [JsonProperty("controller_mac")]
        public string ControllerMac { get; private set; }

        [JsonProperty("part")]
        public PartModel Part { get; private set; }

        [JsonProperty("create_date")]
        public DateTime CreateDate { get; private set; } = DateTime.Now;
    }
}