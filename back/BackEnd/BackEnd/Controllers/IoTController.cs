using System.Net.Http;
using System.Web;
using System.Web.Http;

using Autofac;

using Models;

using ServiceHolder;
using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace BackEnd.Controllers
{
    public class IoTController : ApiController
    {
        private static readonly IBuildService BuildService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IBuildService>();

        private void CheckUserAgent(HttpRequestMessage request)
        {
            if (!Request.Properties.ContainsKey("MS_HttpContext"))
                throw new ForbiddenException("controller");

            HttpContextWrapper context = (Request.Properties["MS_HttpContext"] as HttpContextWrapper);
            if(context.Request.UserAgent != "ESP8266")
                throw new ForbiddenException("controller");
        }

        [HttpPost]
        public HttpResponseMessage Ping([FromBody] ControllerPingDto controllerPingDto)
        {
            return Request.ExecuteProtectedAndWrapResult<ControllerPingDto, IndicatorMapModel>(
                dto => { CheckUserAgent(Request); return BuildService.HandlePing(dto); }, 
                ModelState, controllerPingDto
            );
        }

        [HttpPost]
        public HttpResponseMessage HandleStepProbe([FromBody] StepProbeDto probeDto)
        {
            return Request.ExecuteProtectedAndWrapResult<StepProbeDto, StepProbeResultModel>(
                dto => { CheckUserAgent(Request); return BuildService.HandleStepProbe(dto); },
                ModelState, probeDto
            );
        }
    }
}