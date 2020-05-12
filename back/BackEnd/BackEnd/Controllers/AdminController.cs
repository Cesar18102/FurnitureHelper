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
    public class AdminController : ApiController
    {
        private IAdminService AdminService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IAdminService>();

        [HttpPost]
        public HttpResponseMessage AddAdmin([FromBody] AddAdminDto adminDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddAdminDto, AdminModel>(
                dto => AdminService.AddAdmin(dto),
                ModelState, adminDto
            );
        }

        [HttpPost]
        public HttpResponseMessage AddSuperAdmin([FromBody] AddAdminDto adminDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddAdminDto, SuperAdminModel>(
                dto => AdminService.AddSuperAdmin(dto),
                ModelState, adminDto
            );
        }
    }
}
