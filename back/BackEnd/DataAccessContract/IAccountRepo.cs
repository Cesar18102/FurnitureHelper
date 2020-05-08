using System;

using Models;

namespace DataAccessContract
{
    public interface IAccountRepo : IRepo<AccountModel>
    {
        AccountModel GetByLogin(string login);
        AccountModel GetByEmail(string email);

        /*AccountModel AddAccountExtension(AccountExtensionModel accountExtension);
        AccountModel UpdateAccountExtensionLastUsedDate(int accountExtensionId, DateTime lastUsedDate);

        AccountExtensionModel GetExtensionById(int id);*/
    }
}
