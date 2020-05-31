using System.Net.Http;
using System.Web.Http;

using Autofac;

using Models;

using ServiceHolder;
using ServicesContract;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    public class BuildController : ApiController
    {
        private static readonly IBuildService BuildService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IBuildService>();

        [HttpPost]
        public HttpResponseMessage InitBuildSession([FromBody] StartBuildDto startBuildDto)
        {
            return Request.ExecuteProtectedAndWrapResult<StartBuildDto, BuildSessionModel>(
                dto => BuildService.InitBuildSession(dto),
                ModelState, startBuildDto
            );
        }

        [HttpPost]
        public HttpResponseMessage GetBuildSession([FromBody] SessionDto session)
        {
            return Request.ExecuteProtectedAndWrapResult<SessionDto, BuildSessionModel>(
                dto => BuildService.GetBuildSession(dto),
                ModelState, session
            );
        }

        [HttpPost]
        public HttpResponseMessage GetCurrentGlobalStep([FromBody] BuildSessionDto buildSession)
        {
            return Request.ExecuteProtectedAndWrapResult<BuildSessionDto, GlobalPartsConnectionModel>(
                dto => BuildService.GetCurrentGlobalStep(dto),
                ModelState, buildSession
            );
        }

        [HttpPost]
        public HttpResponseMessage GetCurrentStep([FromBody] BuildSessionDto buildSession)
        {
            return Request.ExecuteProtectedAndWrapResult<BuildSessionDto, TwoPartsConnectionModel>(
                dto => BuildService.GetCurrentStep(dto),
                ModelState, buildSession
            );
        }

        [HttpPost]
        public HttpResponseMessage GetStepProbes([FromBody] BuildSessionDto buildSession)
        {
            return Request.ExecuteProtectedAndWrapResult<BuildSessionDto, StepProbeResultModel>(
                dto => BuildService.GetStepProbeResults(dto),
                ModelState, buildSession
            );
        }
    }
}
