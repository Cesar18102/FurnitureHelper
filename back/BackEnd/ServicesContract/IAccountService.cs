using DataTypes.Dto;
using DataTypes.Models;

namespace ServicesContract
{
    public interface IAccountService
    {
        AccountModel Create(AccountDto dto);
        AccountModel Update(int id, AccountDto dto);
    }
}
