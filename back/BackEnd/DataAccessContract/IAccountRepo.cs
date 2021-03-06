﻿using System;

using Models;

namespace DataAccessContract
{
    public interface IAccountRepo : IRepo<AccountModel>
    {
        AccountModel GetByLogin(string login);
        AccountModel GetByEmail(string email);
        AccountModel GetByOwnedPartMac(string mac);
    }
}
