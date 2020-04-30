using System.Collections.Generic;

namespace BackEnd.Models
{
    public class UserSell
    {
        public int Id { get; private set; }
        public AccountExtension BuyerAccountExtension { get; private set; }
        public AccountExtension SellerAccountExtension { get; private set; }
        public IDictionary<ConcretePart, float> ConcretePartsPriced { get; private set; }
    }
}