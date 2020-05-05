using Models;

using DataAccessContract;
using DataAccess.Entities;

using System.Linq;

namespace DataAccess.RepoImplementation
{
    public class SuperAdminRepo : RepoBase<SuperAdminModel, SuperAdminEntity>, ISuperAdminRepo
    {
        public SuperAdminRepo(FurnitureHelperContext context) : base(context) { }

        public bool IsSuperAdmin(int accountId)
        {
            return Context.super_admins.FirstOrDefault(super_admin => super_admin.admins.account_id == accountId) != null;
        }
    }
}
