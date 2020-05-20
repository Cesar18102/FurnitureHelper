using System.Threading.Tasks;
using System.Collections.Generic;

namespace ServerAccess
{
    public interface IServer
    {
        Task<TOut> SendGet<TOut>(string url) where TOut : class;
        Task<TOut> SendGet<TOut>(string url, IDictionary<string, string> parameters) where TOut : class;
        Task<TOut> SendPost<TIn, TOut>(string url, TIn body) where TIn : class
                                                             where TOut : class;
    }
}
