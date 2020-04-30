namespace DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("furniture_helper.parts_connection_glues")]
    public partial class PartsConnectionGlue
    {
        public int connection_id { get; set; }

        public int? glue_part_id { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string comment_text { get; set; }

        public int id { get; set; }

        public virtual FurnitureItemPartsConnection furniture_item_parts_connections { get; set; }

        public virtual Part parts { get; set; }
    }
}
