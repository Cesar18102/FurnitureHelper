using System.Collections.Generic;

using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IFurnitureService
    {
        FurnitureItemModel RegisterFurnitureItem(AddFurnitureDto dto);
        FurnitureItemModel UpdateFurnitureItem(UpdateFurnitureDto dto);
        FurnitureItemModel UpdateConnections(ConnectionsDto dto);

        IEnumerable<FurnitureItemModel> GetAll();
        IEnumerable<FurnitureItemModel> GetBuildList(SessionDto session);
        bool CanBuild(SessionDto session, int furnitureItemId);

        InvariantPartStore GetPartStore(int furnitureItemId);
    }
}
