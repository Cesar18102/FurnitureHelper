namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.colors")]
    public partial class PartColorEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartColorEntity()
        {
            concrete_parts = new HashSet<ConcretePartEntity>();
            materials = new HashSet<MaterialEntity>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string red { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string green { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(1)]
        public string blue { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConcretePartEntity> concrete_parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialEntity> materials { get; set; }

        
    }
}
