namespace Models
{
    public class SellPositionModel : IModel
    {
        public int Id { get; private set; }
        public float Price { get; private set; }
        public ConcretePartModel ConcretePart { get; private set; }

        public SellPositionModel() { }

        public SellPositionModel(float price, ConcretePartModel concretePart)
        {
            Price = price;
            ConcretePart = concretePart;
        }
    }
}
