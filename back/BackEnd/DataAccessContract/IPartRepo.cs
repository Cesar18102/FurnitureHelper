using Models;

namespace DataAccessContract
{
    public interface IPartRepo : IRepo<PartModel>
    {
        int GetCountOfStored(int partId);
    }
}
