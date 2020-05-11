using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class ManufacturerSellRepo : RepoBase<SellModel, ManufacturerSellEntity>, IManufacturerSellsRepo
    {
        public ManufacturerSellRepo(FurnitureHelperContext context) : base(context) { }
    }
}
