namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.ownings")]
    public partial class OwningEntity : IEntity
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public int concrete_part_id { get; set; }
        public virtual AccountEntity accounts { get; set; }
        public virtual ConcretePartEntity concrete_parts { get; set; }

        
    }
}
