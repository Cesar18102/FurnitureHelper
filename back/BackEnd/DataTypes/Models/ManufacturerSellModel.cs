using System.Linq;
using System.Collections.Generic;

namespace Models
{
    public class ManufacturerSellModel : IModel
    {
        public int Id { get; private set; }
        public AccountExtensionModel BuyerAccountExtension { get; private set; }
        public IDictionary<ConcretePartModel, float> ConcretePartsPriced { get; private set; }

        public float TotalPrice => ConcretePartsPriced.Sum(order => order.Value);

        public ManufacturerSellModel(AccountExtensionModel extension, IDictionary<ConcretePartModel, float> orders)
        {
            BuyerAccountExtension = extension;
            ConcretePartsPriced = orders;
        }
    }
}