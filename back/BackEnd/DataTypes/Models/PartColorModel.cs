using System.Drawing;

namespace Models
{
    public class PartColorModel : IModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public string Description { get; private set; }
    }
}