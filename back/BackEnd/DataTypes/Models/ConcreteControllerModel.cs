namespace Models
{
    public class ConcreteControllerModel : EmbedControllerPositionModel, IModel
    {
        public int ControllerId { get; private set; }
        public byte[] MAC { get; private set; }
    }
}