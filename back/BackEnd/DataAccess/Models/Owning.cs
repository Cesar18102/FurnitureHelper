namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.ownings")]
    public partial class Owning
    {
        public int id { get; set; }

        public int account_id { get; set; }

        public int concrete_part_id { get; set; }

        public virtual Account accounts { get; set; }

        public virtual ConcretePart concrete_parts { get; set; }
    }
}
