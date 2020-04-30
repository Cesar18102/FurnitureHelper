namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.accounts")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            accounts_extensions = new HashSet<AccountExtension>();
            ownings = new HashSet<Owning>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string login { get; set; }

        [Required]
        [StringLength(256)]
        public string pwd { get; set; }

        [Required]
        [StringLength(256)]
        public string email { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(64)]
        public string first_name { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(64)]
        public string last_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountExtension> accounts_extensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Owning> ownings { get; set; }
    }
}
