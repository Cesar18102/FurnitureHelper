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

        PartStore GetPartStore(int furnitureItemId);
        IEnumerable<FurnitureItemModel> GetAll();
        IEnumerable<FurnitureItemModel> GetBuildList(SessionDto session);
    }
}
