using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IAccountService
    {
        AccountModel SignUp(SignUpDto dto);
        SessionModel LogIn(LogInDto dto);

        AccountModel Update(UpdateAccountDto dto);
        AccountModel GetInfo(SessionDto dto);
    }
}
