using Models;

namespace DataAccessContract
{
    public interface IConcretePartRepo : IRepo<ConcretePartModel>
    {
        ConcretePartModel GetPartByMac(string mac);
    }
}
