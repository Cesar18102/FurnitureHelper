namespace Models
{
    public class ConnectionGlueModel : IModel
    {
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public PartModel GluePart { get; private set; }
    }
}