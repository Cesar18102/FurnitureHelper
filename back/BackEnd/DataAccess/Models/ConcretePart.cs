namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.concrete_parts")]
    public partial class ConcretePart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConcretePart()
        {
            concrete_controllers = new HashSet<ConcreteController>();
            manufacturer_sell_positions = new HashSet<ManufacturerSellPosition>();
            ownings = new HashSet<Owning>();
            user_sell_positions = new HashSet<UserSellPosition>();
        }

        public int id { get; set; }

        public int part_id { get; set; }

        public int material_id { get; set; }

        public int color_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime create_date { get; set; }

        public virtual Color colors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConcreteController> concrete_controllers { get; set; }

        public virtual Part parts { get; set; }

        public virtual Material materials { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufacturerSellPosition> manufacturer_sell_positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Owning> ownings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSellPosition> user_sell_positions { get; set; }
    }
}
