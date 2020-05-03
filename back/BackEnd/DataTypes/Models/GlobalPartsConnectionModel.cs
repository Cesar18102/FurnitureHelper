using System.Collections.Generic;

namespace Models
{
    public class GlobalPartsConnectionModel : IModel
    {
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public int OrderNumber { get; private set; }
        public IEnumerable<TwoPartsConnectionModel> SubConnections { get; private set; }
        public IEnumerable<ConnectionGlueModel> GlobalConnectionGlues { get; private set; }
    }
}