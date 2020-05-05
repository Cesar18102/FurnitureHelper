using System.Linq;
using System.Data.Entity;

using AutoMapper;

using Models;

using DataAccess.Entities;
using DataAccessContract;

namespace DataAccess.RepoImplementation
{
    public class AdminRepo : RepoBase<AdminModel, AdminEntity>, IAdminRepo
    {
        public AdminRepo(FurnitureHelperContext context) : base(context) { }

        protected override void SingleInclude(AdminEntity entity)
        {
            Context.Entry<AdminEntity>(entity).Reference(admin => admin.accounts).Load();
        }

        protected override void WholeInclude()
        {
            Context.admins.Include(admin => admin.accounts);
        }

        public bool IsAdmin(int accountId)
        {
            return Context.admins.FirstOrDefault(admin => admin.account_id == accountId) != null;
        }

        public AdminModel GetByAccountId(int accountId)
        {
            AdminEntity entity = Context.admins.FirstOrDefault(admin => admin.account_id == accountId);
            return entity == null ? null : Mapper.Map<AdminEntity, AdminModel>(entity);
        }
    }
}
