using System.Collections.Generic;

namespace Models
{
    public class TwoPartsConnectionModel : IModel
    {
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public int OrderNumber { get; private set; }
        public EmbedControllerPositionModel ControllerPosition { get; private set; }
        public EmbedControllerPositionModel ControllerPositionOther { get; private set; }
        public IEnumerable<ConnectionGlueModel> ConnectionGlues { get; private set; }   
    }
}