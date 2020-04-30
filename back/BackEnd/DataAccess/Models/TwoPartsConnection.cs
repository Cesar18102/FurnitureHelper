namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.two_parts_connection")]
    public partial class TwoPartsConnection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TwoPartsConnection()
        {
            two_parts_connection_glues = new HashSet<TwoPartsConnectionGlue>();
        }

        public int id { get; set; }

        public int global_connection_id { get; set; }

        public int part_controller_id { get; set; }

        public int part_controller_other_id { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string comment_text { get; set; }

        public int order_number { get; set; }

        public virtual FurnitureItemPartsConnection furniture_item_parts_connections { get; set; }

        public virtual PartControllerEmbedRelativePosition part_controllers_embed_relative_positions { get; set; }

        public virtual PartControllerEmbedRelativePosition part_controllers_embed_relative_positions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionGlue> two_parts_connection_glues { get; set; }
    }
}
