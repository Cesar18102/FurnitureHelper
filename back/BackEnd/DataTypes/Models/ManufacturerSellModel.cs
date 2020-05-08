using System;
using System.Linq;
using System.Collections.Generic;

namespace Models
{
    public class ManufacturerSellModel : IModel
    {
        public int Id { get; private set; }
        public DateTime SellDate { get; set; } = DateTime.Now;
        public AccountExtensionModel BuyerAccountExtension { get; private set; }
        public ICollection<SellPositionModel> SellPositions { get; private set; } = new List<SellPositionModel>();

        public float TotalPrice => SellPositions.Sum(order => order.Price);

        public ManufacturerSellModel() { }

        public ManufacturerSellModel(AccountExtensionModel extension)
        {
            BuyerAccountExtension = extension;
        }
    }
}