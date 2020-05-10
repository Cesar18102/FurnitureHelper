namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.furniture_items")]
    public partial class FurnitureItemEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FurnitureItemEntity()
        {
            furniture_item_parts_connections = new HashSet<FurnitureItemPartsConnectionEntity>();
            used_parts = new HashSet<UsedPartEntity>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FurnitureItemPartsConnectionEntity> furniture_item_parts_connections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsedPartEntity> used_parts { get; set; }
    }
}
