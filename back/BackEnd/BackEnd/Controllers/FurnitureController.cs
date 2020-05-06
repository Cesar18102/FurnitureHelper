using System.Net.Http;
using System.Web.Http;

using Autofac;

using Models;
using ServiceHolder;
using ServicesContract;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    public class FurnitureController : ApiController
    {
        private IFurnitureService FurnitureService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IFurnitureService>();

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddFurnitureItemDto furnitureDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddFurnitureItemDto, FurnitureItemModel>(
                dto => FurnitureService.RegisterFurnitureItem(dto),
                ModelState, furnitureDto
            );
        }

        /*[HttpPost]
        public HttpResponseMessage Update([FromBody] UpdatePartDto partDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdatePartDto, PartModel>(
                dto => PartService.UpdatePart(dto),
                ModelState, partDto
            );
        }*/
    }
}
