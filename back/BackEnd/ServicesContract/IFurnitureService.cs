using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IFurnitureService
    {
        FurnitureItemModel RegisterFurnitureItem(AddFurnitureDto dto);
        FurnitureItemModel UpdateFurnitureItem(UpdateFurnitureDto dto);
    }
}
