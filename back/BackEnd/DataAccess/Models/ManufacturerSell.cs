namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.manufacturer_sells")]
    public partial class ManufacturerSell
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ManufacturerSell()
        {
            manufacturer_sell_positions = new HashSet<ManufacturerSellPosition>();
        }

        public int id { get; set; }

        public int accounts_extension_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime sell_date { get; set; }

        public virtual AccountExtension accounts_extensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufacturerSellPosition> manufacturer_sell_positions { get; set; }
    }
}
