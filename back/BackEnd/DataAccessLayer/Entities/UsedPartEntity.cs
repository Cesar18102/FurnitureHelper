namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.used_parts")]
    public partial class UsedPartEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsedPartEntity()
        {
            two_parts_connection = new HashSet<TwoPartsConnectionEntity>();
            two_parts_connection1 = new HashSet<TwoPartsConnectionEntity>();
        }

        public int id { get; set; }

        public int? part_id { get; set; }

        public int furniture_item_id { get; set; }

        public virtual FurnitureItemEntity furniture_items { get; set; }

        public virtual PartEntity parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionEntity> two_parts_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionEntity> two_parts_connection1 { get; set; }
    }
}
