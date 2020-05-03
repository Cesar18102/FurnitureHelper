namespace DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("furniture_helper.manufacturer_sell_positions")]
    public partial class ManufacturerSellPositionEntity : IEntity
    {
        public int id { get; set; }
        public int manufacturer_sell_id { get; set; }
        public int concrete_part_id { get; set; }
        public float price { get; set; }
        public virtual ConcretePartEntity concrete_parts { get; set; }
        public virtual ManufacturerSellEntity manufacturer_sells { get; set; }

        
    }
}
