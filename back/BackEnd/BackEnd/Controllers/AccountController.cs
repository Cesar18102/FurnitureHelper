using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Cors;

using Autofac;

using ServiceHolder;
using ServicesContract;

using Models;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AccountController : ApiController
    {
        private IAccountService AccountService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<IAccountService>();

        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] SignUpDto signUpDto)
        {
            return Request.ExecuteProtectedAndWrapResult<SignUpDto, AccountModel>(
                dto => AccountService.SignUp(dto),
                ModelState, signUpDto
            );
        }

        [HttpPost]
        public HttpResponseMessage LogIn([FromBody] LogInDto logInDto)
        {
            return Request.ExecuteProtectedAndWrapResult<LogInDto, SessionModel>(
                dto => AccountService.LogIn(dto),
                ModelState, logInDto
            );
        }

        [HttpPost]
        public HttpResponseMessage UpdateAccount([FromBody] UpdateAccountDto updateAccountDto)
        {
            return Request.ExecuteProtectedAndWrapResult<UpdateAccountDto, AccountModel>(
                dto => AccountService.Update(dto),
                ModelState, updateAccountDto
            );
        }

        [HttpPost]
        public HttpResponseMessage GetInfo([FromBody] SessionDto sessionDto)
        {
            return Request.ExecuteProtectedAndWrapResult<SessionDto, AccountModel>(
                dto => AccountService.GetInfo(dto),
                ModelState, sessionDto
            );
        }
    }
}
