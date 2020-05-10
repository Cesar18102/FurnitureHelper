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

        [HttpPost]
        public HttpResponseMessage UpdateConnections([FromBody] ConnectionsDto connectionsDto)
        {
            return Request.ExecuteProtectedAndWrapResult<ConnectionsDto, FurnitureItemModel>(
                dto => FurnitureService.UpdateConnections(dto),
                ModelState, connectionsDto
            );
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.ExecuteProtectedAndWrapResult<FurnitureItemModel>(FurnitureService.GetAll);
        }

        [HttpPost]
        public HttpResponseMessage GetBuildList(SessionDto sessionDto)
        {
            //TODO
            return null;
        }
    }
}
