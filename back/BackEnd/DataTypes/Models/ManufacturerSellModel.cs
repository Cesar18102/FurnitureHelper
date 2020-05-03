using System.Collections.Generic;

namespace Models
{
    public class ManufacturerSellModel : IModel
    {
        public int Id { get; private set; }
        public AccountExtensionModel BuyerAccountExtension { get; private set; }
        public IDictionary<ConcretePartModel, float> ConcretePartsPriced { get; private set; }   
    }
}