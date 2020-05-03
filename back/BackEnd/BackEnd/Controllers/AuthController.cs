using System.Web.Http;
using System.Net.Http;

using Autofac;

using ServiceHolder;
using ServicesContract;

using DataTypes.Dto;
using DataTypes.Models;

namespace BackEnd.Controllers
{
    public class AuthController : ApiController
    {
        private IAccountService AccountService = ServiceDependencyHolder.ServicesDependencies.Resolve<IAccountService>();

        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] AccountDto accountDto)
        {
            return CommonRequestProcessor.ExecuteCommonRequestProccessing<AccountModel, AccountDto>(
                () => AccountService.Create(accountDto), Request
            );
        }

        [HttpPost]
        public HttpResponseMessage UpdateAccount([FromBody] AccountDto accountDto, [FromUri] int id)
        {
            return CommonRequestProcessor.ExecuteCommonRequestProccessing<AccountModel, AccountDto>(
                () => AccountService.Update(id, accountDto), Request
            );
        }
    }
}
