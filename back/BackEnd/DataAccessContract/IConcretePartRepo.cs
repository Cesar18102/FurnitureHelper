using Models;

namespace DataAccessContract
{
    public interface IConcretePartRepo : IRepo<ConcretePartModel>
    {
        ConcreteControllerModel GetEmbeddedControllerByMac(string mac);
    }
}
