using Models;

namespace DataAccessContract
{
    public interface IAdminRepo : IRepo<AdminModel>
    {
        bool IsAdmin(int accountId);
        AdminModel GetByAccountId(int accountId);
    }
}
