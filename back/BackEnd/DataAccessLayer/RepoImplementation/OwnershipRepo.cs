using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class OwnershipRepo : RepoBase<OwnershipModel, OwningEntity>, IOwnershipRepo
    {
        public OwnershipRepo(FurnitureHelperContext context) : base(context) { }
    }
}
