using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class AccountExtensionRepo : RepoBase<AccountExtensionModel, AccountExtensionEntity>, IAccountExtensionRepo
    {
        public AccountExtensionRepo(FurnitureHelperContext context) : base(context) { }
    }
}
