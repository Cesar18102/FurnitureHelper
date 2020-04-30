using System.Collections.Generic;

namespace BackEnd.Models
{
    public class GlobalPartsConnection
    {
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public int OrderNumber { get; private set; }
        public IEnumerable<TwoPartsConnection> SubConnections { get; private set; }
        public IEnumerable<ConnectionGlue> GlobalConnectionGlues { get; private set; }
    }
}