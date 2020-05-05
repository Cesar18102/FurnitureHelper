namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.parts")]
    public partial class PartEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartEntity()
        {
            concrete_parts = new HashSet<ConcretePartEntity>();
            part_controllers_embed_relative_positions = new HashSet<PartControllerEmbedRelativePositionEntity>();
            parts_connection_glues = new HashSet<PartsConnectionGlueEntity>();
            two_parts_connection_glues = new HashSet<TwoPartsConnectionGlueEntity>();
            materials = new HashSet<MaterialEntity>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(256)]
        public string name { get; set; }

        [Required]
        [StringLength(512)]
        public string model_url { get; set; }

        public float price { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConcretePartEntity> concrete_parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartControllerEmbedRelativePositionEntity> part_controllers_embed_relative_positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartsConnectionGlueEntity> parts_connection_glues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionGlueEntity> two_parts_connection_glues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialEntity> materials { get; set; }
    }
}
