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
    public class FurnitureController : ApiController
    {
        private static readonly IFurnitureService FurnitureService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IFurnitureService>();
        private static readonly IPartService PartService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IPartService>();

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
            return Request.ExecuteProtectedAndWrapResult<SessionDto, FurnitureItemModel>(
                dto => FurnitureService.GetBuildList(dto),
                ModelState, sessionDto
            );
        }

        [HttpGet]
        public HttpResponseMessage GetPartStoreForFurniture(int furnitureItemId)
        {
            return Request.ExecuteProtectedAndWrapResult<InvariantPartStore>(() => FurnitureService.GetPartStore(furnitureItemId));
        }
    }
}
