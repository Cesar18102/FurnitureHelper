using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class ManufacturerSellRepo : RepoBase<ManufacturerSellModel, ManufacturerSellEntity>, IManufacturerSellsRepo
    {
        public ManufacturerSellRepo(FurnitureHelperContext context) : base(context) { }
    }
}
