namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.concrete_parts")]
    public partial class ConcretePartEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConcretePartEntity()
        {
            manufacturer_sell_positions = new HashSet<ManufacturerSellPositionEntity>();
            ownings = new HashSet<OwningEntity>();
            user_sell_positions = new HashSet<UserSellPositionEntity>();
        }

        public int id { get; set; }
        public int part_id { get; set; }
        public int material_id { get; set; }
        public int color_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? last_sell_date { get; set; }

        [Column(TypeName = "char")]
        [StringLength(17)]
        public string controller_mac { get; set; }

        [Column(TypeName = "date")]
        public DateTime create_date { get; set; }

        public virtual PartColorEntity colors { get; set; }

        public virtual PartEntity parts { get; set; }

        public virtual MaterialEntity materials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufacturerSellPositionEntity> manufacturer_sell_positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OwningEntity> ownings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSellPositionEntity> user_sell_positions { get; set; }
    }
}
