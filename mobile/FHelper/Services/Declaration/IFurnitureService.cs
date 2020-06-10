using System.Threading.Tasks;
using System.Collections.Generic;

using Models;

namespace Services.Declaration
{
    public interface IFurnitureService
    {
        Task<IEnumerable<FurnitureItemDto>> GetFurnitureItems();
    }
}
