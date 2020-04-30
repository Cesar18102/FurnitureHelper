namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.accounts_extensions")]
    public partial class AccountExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountExtension()
        {
            manufacturer_sells = new HashSet<ManufacturerSell>();
            user_sells = new HashSet<UserSell>();
            user_sells1 = new HashSet<UserSell>();
        }

        public int id { get; set; }

        public int account_id { get; set; }

        [Required]
        [StringLength(32)]
        public string phone { get; set; }

        [Required]
        [StringLength(256)]
        public string address { get; set; }

        [Column(TypeName = "date")]
        public DateTime last_used { get; set; }

        public virtual Account accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManufacturerSell> manufacturer_sells { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSell> user_sells { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSell> user_sells1 { get; set; }
    }
}
