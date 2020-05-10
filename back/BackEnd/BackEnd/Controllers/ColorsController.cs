using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

using Autofac;

using Models;
using ServiceHolder;

using ServicesContract;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
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
    }
}
