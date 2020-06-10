using Xamarin.Forms;

using FHelper.Interfaces;
using FHelper.Droid.Implementations;

[assembly : Dependency(typeof(BaseUrl))]
namespace FHelper.Droid.Implementations
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}