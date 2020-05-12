using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using Autofac;

using Models;

using ServicesContract;
using ServicesContract.Dto;

using ServiceHolder;

namespace BackEnd.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PartController : ApiController
    {
        private static readonly IPartService PartService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IPartService>();

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddPartDto partDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddPartDto, PartModel>(
                dto => PartService.RegisterPart(dto),
                ModelState, partDto
            );
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] UpdatePartDto partDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdatePartDto, PartModel>(
                dto => PartService.UpdatePart(dto),
                ModelState, partDto
            );
        }

        [HttpPost]
        public HttpResponseMessage AddConcretePart([FromBody] AddConcretePartDto concretePartDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddConcretePartDto, ConcretePartModel>(
                dto => PartService.RegisterConcretePart(dto),
                ModelState, concretePartDto
            );
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.ExecuteProtectedAndWrapResult<PartStore>(PartService.GetStore);
        }

        [HttpGet]
        public HttpResponseMessage GetBids()
        {
            return Request.ExecuteProtectedAndWrapResult<PartStore>(PartService.GetUserBids);
        }

        [HttpPost]
        public HttpResponseMessage GetOwned(SessionDto session)
        {
            return Request.ExecuteProtectedAndWrapResult<SessionDto, PartStore>(
                sess => PartService.GetOwned(sess), 
                ModelState, session
            );
        }

        [HttpPost]
        public HttpResponseMessage GetOwnedConcrete(SessionDto session)
        {
            return Request.ExecuteProtectedAndWrapResult(
                sess => PartService.GetOwnedConcrete(sess),
                ModelState, session
            );
        }
    }
}
