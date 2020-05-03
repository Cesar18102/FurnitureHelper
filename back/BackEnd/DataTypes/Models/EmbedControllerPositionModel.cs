namespace Models
{
    public class EmbedControllerPositionModel : IModel
    {
        public int Id { get; private set; }
        public float PosX { get; private set; }
        public float PosY { get; private set; }
        public float PosZ { get; private set; }   
    }
}