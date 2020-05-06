using System.Linq;

using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class ConcretePartRepo : RepoBase<ConcretePartModel, ConcretePartEntity>, IConcretePartRepo
    {
        public ConcretePartRepo(FurnitureHelperContext context) : base(context) { }

        public ConcreteControllerModel GetEmbeddedControllerByMac(string mac)
        {
            ConcreteControllerEntity controller = Context.concrete_controllers.FirstOrDefault(ctrl => ctrl.mac == mac);
            return controller == null ? null : Mapper.Map<ConcreteControllerEntity, ConcreteControllerModel>(controller);
        }
    }
}
