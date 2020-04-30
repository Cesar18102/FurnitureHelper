using System.Collections.Generic;

namespace BackEnd.Models
{
    public class TwoPartsConnection
    {
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public int OrderNumber { get; private set; }
        public EmbedControllerPosition ControllerPosition { get; private set; }
        public EmbedControllerPosition ControllerPositionOther { get; private set; }
        public IEnumerable<ConnectionGlue> ConnectionGlues { get; private set; }
    }
}