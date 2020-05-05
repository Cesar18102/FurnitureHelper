using System.Net.Http;
using System.Web.Http;

using Autofac;

using Models;

using ServicesContract;
using ServicesContract.Dto;

using ServiceHolder;

namespace BackEnd.Controllers
{
    public class PartController : ApiController
    {
        private IPartService PartService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IPartService>();

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
    }
}
