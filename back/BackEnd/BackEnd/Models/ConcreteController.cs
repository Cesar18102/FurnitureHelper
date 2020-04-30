namespace BackEnd.Models
{
    public class ConcreteController : EmbedControllerPosition
    {
        public int ControllerId { get; private set; }
        public byte[] MAC { get; private set; }
    }
}