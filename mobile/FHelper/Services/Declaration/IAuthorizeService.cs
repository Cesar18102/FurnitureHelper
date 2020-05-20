using System.Threading.Tasks;

using Models.Dto;
using Models.Forms;

namespace Services.Declaration
{
    public interface IAuthorizeService
    {
        Task<SessionDto> SignUp(SignUpForm signUpForm);
        Task<SessionDto> LogIn(LogInForm logInForm);
    }
}
