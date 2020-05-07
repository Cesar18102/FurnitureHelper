namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.part_controllers_embed_relative_positions")]
    public partial class PartControllerEmbedRelativePositionEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PartControllerEmbedRelativePositionEntity()
        {
            two_parts_connection = new HashSet<TwoPartsConnectionEntity>();
            two_parts_connection1 = new HashSet<TwoPartsConnectionEntity>();
        }

        public int id { get; set; }
        public int part_id { get; set; }
        public float pos_x { get; set; }
        public float pos_y { get; set; }
        public float pos_z { get; set; }
        public int indicator_pin_number { get; set; }
        public int reader_pin_number { get; set; }

        public virtual PartEntity parts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionEntity> two_parts_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TwoPartsConnectionEntity> two_parts_connection1 { get; set; }

        
    }
}
