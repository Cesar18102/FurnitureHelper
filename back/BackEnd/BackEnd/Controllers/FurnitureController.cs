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
        public HttpResponseMessage Add([FromBody] AddFurnitureDto furnitureDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddFurnitureDto, FurnitureItemModel>(
                dto => FurnitureService.RegisterFurnitureItem(dto),
                ModelState, furnitureDto
            );
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] UpdateFurnitureDto furnitureDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdateFurnitureDto, FurnitureItemModel>(
                dto => FurnitureService.UpdateFurnitureItem(dto),
                ModelState, furnitureDto
            );
        }
    }
}
