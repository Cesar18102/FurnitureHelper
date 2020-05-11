using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IConcretePartRepo : IRepo<ConcretePartModel>
    {
        ConcretePartModel GetPartByMac(string mac);
        IEnumerable<ConcretePartModel> GetOwnedByUser(int userId);

        IEnumerable<ConcretePartModel> GetUnsoldParts();
        IEnumerable<ConcretePartModel> GetForSellParts();

        IEnumerable<ConcretePartModel> MarkPartsForSell(IEnumerable<ConcretePartModel> parts);
        IEnumerable<ConcretePartModel> UnmarkPartsForSell(IEnumerable<ConcretePartModel> parts);
    }
}
