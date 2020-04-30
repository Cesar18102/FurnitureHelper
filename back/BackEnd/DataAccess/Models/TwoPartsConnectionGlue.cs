namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.two_parts_connection_glues")]
    public partial class TwoPartsConnectionGlue
    {
        public int two_parts_connection_id { get; set; }

        public int? glue_part_id { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string comment_text { get; set; }

        public int id { get; set; }

        public virtual Part parts { get; set; }

        public virtual TwoPartsConnection two_parts_connection { get; set; }
    }
}
