using System.Web.Http;
using System.Collections.Generic;

namespace BackEnd.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            /*AutoMapper.MapperConfiguration cnf = new AutoMapper.MapperConfiguration(config => config.CreateMap<DataAccess.Account, Models.Account>());
            AutoMapper.Mapper mapper = new AutoMapper.Mapper(cnf);
            Models.Account acc = mapper.Map<Models.Account>(new DataAccess.Account() { id = 1, email = "asdasd", login = "cesar", pwd = "123456" });*/

            return Json<object>(new object());
        }
    }
}
