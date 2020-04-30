namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.furniture_items")]
    public partial class FurnitureItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FurnitureItem()
        {
            furniture_item_parts_connections = new HashSet<FurnitureItemPartsConnection>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FurnitureItemPartsConnection> furniture_item_parts_connections { get; set; }
    }
}
