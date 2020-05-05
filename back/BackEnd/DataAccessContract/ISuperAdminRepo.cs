using Models;

namespace DataAccessContract
{
    public interface ISuperAdminRepo : IRepo<SuperAdminModel>
    {
        bool IsSuperAdmin(int accountId);
    }
}
