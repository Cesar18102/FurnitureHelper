using System.Threading.Tasks;

using Models.Dto.PartStore;

namespace Services.Declaration
{
    public interface IPartService
    {
        Task<PartStoreDto> GetParts();
    }
}
