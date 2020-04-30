namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.concrete_controllers")]
    public partial class ConcreteController
    {
        public int id { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(6)]
        public string mac { get; set; }

        public int concrete_part_id { get; set; }

        public int embed_position_id { get; set; }

        public virtual ConcretePart concrete_parts { get; set; }

        public virtual PartControllerEmbedRelativePosition part_controllers_embed_relative_positions { get; set; }
    }
}
