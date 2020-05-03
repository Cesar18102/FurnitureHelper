namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.user_sells")]
    public partial class UserSellEntity : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserSellEntity()
        {
            user_sell_positions = new HashSet<UserSellPositionEntity>();
        }

        public int id { get; set; }
        public int accounts_extension_id { get; set; }
        public int accounts_extension_other_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime sell_date { get; set; }

        public virtual AccountExtensionEntity accounts_extensions { get; set; }
        public virtual AccountExtensionEntity accounts_extensions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSellPositionEntity> user_sell_positions { get; set; }

        
    }
}
