using System.Drawing;

namespace BackEnd.Models
{
    public class PartColor
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public string Description { get; private set; }
    }
}