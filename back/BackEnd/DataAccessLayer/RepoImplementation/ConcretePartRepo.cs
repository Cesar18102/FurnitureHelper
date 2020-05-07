using System.Linq;
using System.Collections.Generic;

using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class ConcretePartRepo : RepoBase<ConcretePartModel, ConcretePartEntity>, IConcretePartRepo
    {
        public ConcretePartRepo(FurnitureHelperContext context) : base(context) { }

        public ConcretePartModel GetPartByMac(string mac)
        {
            ConcretePartEntity part = Context.concrete_parts.FirstOrDefault(ctrl => ctrl.controller_mac.ToUpper() == mac.ToUpper());
            return part == null ? null : Mapper.Map<ConcretePartEntity, ConcretePartModel>(part);
        }

        public IEnumerable<ConcretePartModel> GetManufacturerPartsForSelling(int partId, int amount, IEnumerable<int> reservedIds)
        {
            List<ConcretePartEntity> parts = Context.concrete_parts.Where(part => 
                part.part_id == partId && 
                part.last_sell_date == null && 
                !reservedIds.Contains(part.id)
            ).Take(amount).ToList();

            return Mapper.Map<IEnumerable<ConcretePartEntity>, IEnumerable<ConcretePartModel>>(parts);
        }
    }
}
