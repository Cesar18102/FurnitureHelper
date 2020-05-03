using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;

namespace BackEnd
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );

            MediaTypeHeaderValue xmlTypeHeader = config.Formatters.XmlFormatter.SupportedMediaTypes
                                                       .FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(xmlTypeHeader);
        }
    }
}
