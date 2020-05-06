using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IFurnitureService
    {
        FurnitureItemModel RegisterFurnitureItem(AddFurnitureItemDto dto);
        //FurnitureItemModel UpdateFurnitureItem(UpdateFurnitureItemDto dto);
    }
}
