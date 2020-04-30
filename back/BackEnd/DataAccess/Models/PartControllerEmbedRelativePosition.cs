namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.part_controllers_embed_relative_positions")]
    public partial class PartControllerEmbedRelativePosition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartControllerEmbedRelativePosition()
        {
            concrete_controllers = new HashSet<ConcreteController>();
            two_parts_connection = new HashSet<TwoPartsConnection>();
            two_parts_connection1 = new HashSet<TwoPartsConnection>();
        }

        public int id { get; set; }

        public int part_id { get; set; }

        public float pos_x { get; set; }

        public float pos_y { get; set; }

        public float pos_z { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConcreteController> concrete_controllers { get; set; }

        public virtual Part parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnection> two_parts_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnection> two_parts_connection1 { get; set; }
    }
}
