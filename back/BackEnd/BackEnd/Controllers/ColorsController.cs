using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using Autofac;

using Models;
using ServiceHolder;

using ServicesContract;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ColorsController : ApiController
    {
        private IColorService ColorsService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IColorService>();

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddColorDto colorDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddColorDto, PartColorModel>(                 
                dto => ColorsService.RegisterColor(dto),
                ModelState, colorDto
            );
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] UpdateColorDto colorDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdateColorDto, PartColorModel>(
                dto => ColorsService.UpdateColor(dto),
                ModelState, colorDto
            );
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.ExecuteProtectedAndWrapResult<PartColorModel>(() => ColorsService.GetAll());
        }

        [HttpGet]
        public HttpResponseMessage Get(int colorId)
        {
            return Request.ExecuteProtectedAndWrapResult<PartColorModel>(() => ColorsService.Get(colorId));
        }
    }
}
