using System.Linq;

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
    }
}
