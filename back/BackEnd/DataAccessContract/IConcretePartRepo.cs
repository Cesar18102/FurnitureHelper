using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IConcretePartRepo : IRepo<ConcretePartModel>
    {
        ConcretePartModel GetPartByMac(string mac);
        IEnumerable<ConcretePartModel> GetOwnedByUser(int userId);

        IEnumerable<ConcretePartModel> GetStored();
        IEnumerable<ConcretePartModel> GetStored(int partId);
        IEnumerable<ConcretePartModel> GetStored(int partId, int materialId);
        IEnumerable<ConcretePartModel> GetStored(int partId, int materialId, int colorId);

        void MarkInUse(IEnumerable<int> partIds);
        IEnumerable<ConcretePartModel> GetForSellParts();

        IEnumerable<ConcretePartModel> MarkPartsForSell(IEnumerable<ConcretePartModel> parts);
        IEnumerable<ConcretePartModel> UnmarkPartsForSell(IEnumerable<ConcretePartModel> parts);
    }
}
