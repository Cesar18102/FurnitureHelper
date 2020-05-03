using DataTypes.Dto;
using DataTypes.Models;

namespace DataAccessContract
{
    public interface IAccountRepo : IRepo<AccountDto, AccountModel>
    {
        AccountModel GetByLogin(string login);
        AccountModel GetByEmail(string email);
    }
}
