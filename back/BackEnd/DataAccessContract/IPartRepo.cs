using Models;

namespace DataAccessContract
{
    public interface IPartRepo : IRepo<PartModel>
    {
        bool WasBought(int id);
    }
}
