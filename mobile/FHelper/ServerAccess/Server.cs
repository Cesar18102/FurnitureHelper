using Xamarin.Forms;

using ServerAccess;

[assembly: Dependency(typeof(Server))]
namespace ServerAccess
{
    internal class Server : IServer
    {
    }
}
