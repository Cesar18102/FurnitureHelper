namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.furniture_item_parts_connections")]
    public partial class FurnitureItemPartsConnectionEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FurnitureItemPartsConnectionEntity()
        {
            parts_connection_glues = new HashSet<PartsConnectionGlueEntity>();
            two_parts_connection = new HashSet<TwoPartsConnectionEntity>();
        }

        public int id { get; set; }
        public int furniture_item_id { get; set; }
        public int order_number { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string comment_text { get; set; }

        public virtual FurnitureItemEntity furniture_items { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartsConnectionGlueEntity> parts_connection_glues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionEntity> two_parts_connection { get; set; }

        
    }
}
