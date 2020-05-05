namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.super_admins")]
    public partial class SuperAdminEntity : IEntity
    { 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int admin_account_id { get; set; }

        public virtual AdminEntity admins { get; set; }
    }
}
