using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IConcretePartRepo : IRepo<ConcretePartModel>
    {
        ConcretePartModel GetPartByMac(string mac);
        IEnumerable<ConcretePartModel> GetManufacturerPartsForSelling(int partId, int amount, IEnumerable<int> reservedIds);
    }
}
