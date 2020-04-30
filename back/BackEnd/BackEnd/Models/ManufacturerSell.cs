using System.Collections.Generic;

namespace BackEnd.Models
{
    public class ManufacturerSell
    {
        public int Id { get; private set; }
        public AccountExtension BuyerAccountExtension { get; private set; }
        public IDictionary<ConcretePart, float> ConcretePartsPriced { get; private set; }
    }
}