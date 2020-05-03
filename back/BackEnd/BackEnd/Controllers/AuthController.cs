using System.Web.Http;
using System.Net.Http;

using Autofac;

using ServiceHolder;
using ServicesContract;

using Models;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    public class AuthController : ApiController
    {
        private IAccountService AccountService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IAccountService>();

        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] SignUpDto dto)
        {
            return Request.ExecuteProtectedAndWrapResult<AccountModel>(() => AccountService.SignUp(dto));
        }

        [HttpPost]
        public HttpResponseMessage LogIn([FromBody] LogInDto dto)
        {
            return Request.ExecuteProtectedAndWrapResult<SessionModel>(() => AccountService.LogIn(dto));
        }

        [HttpPost]
        public HttpResponseMessage UpdateAccount([FromBody] UpdateAccountDto dto)
        {
            return Request.ExecuteProtectedAndWrapResult<AccountModel>(() => AccountService.Update(dto));
        }
    }
}
