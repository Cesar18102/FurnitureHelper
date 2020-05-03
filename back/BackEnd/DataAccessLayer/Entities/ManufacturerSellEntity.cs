namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.manufacturer_sells")]
    public partial class ManufacturerSellEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ManufacturerSellEntity()
        {
            manufacturer_sell_positions = new HashSet<ManufacturerSellPositionEntity>();
        }

        public int id { get; set; }
        public int accounts_extension_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime sell_date { get; set; }

        public virtual AccountExtensionEntity accounts_extensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufacturerSellPositionEntity> manufacturer_sell_positions { get; set; }

        
    }
}
