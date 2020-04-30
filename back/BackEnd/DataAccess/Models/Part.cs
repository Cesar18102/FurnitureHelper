namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.parts")]
    public partial class Part
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Part()
        {
            concrete_parts = new HashSet<ConcretePart>();
            part_controllers_embed_relative_positions = new HashSet<PartControllerEmbedRelativePosition>();
            parts_connection_glues = new HashSet<PartsConnectionGlue>();
            two_parts_connection_glues = new HashSet<TwoPartsConnectionGlue>();
            materials = new HashSet<Material>();
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
        public virtual ICollection<ConcretePart> concrete_parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartControllerEmbedRelativePosition> part_controllers_embed_relative_positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PartsConnectionGlue> parts_connection_glues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionGlue> two_parts_connection_glues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> materials { get; set; }
    }
}
