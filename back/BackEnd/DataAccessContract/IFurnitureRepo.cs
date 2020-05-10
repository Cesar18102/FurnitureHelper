using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IFurnitureRepo : IRepo<FurnitureItemModel>
    {
        FurnitureItemModel Update(int id, FurnitureItemModel model, IEnumerable<int> usedPartsToRemove, IEnumerable<UsedPartModel> usedPartsToAdd);
        FurnitureItemModel UpdateConnections(int id, IEnumerable<GlobalPartsConnectionModel> connections);
    }
}
