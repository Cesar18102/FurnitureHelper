using System;
using System.Linq;
using System.Data.Entity;

using AutoMapper;

using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class AccountRepo : RepoBase<AccountModel, AccountEntity>, IAccountRepo
    {
        public AccountRepo(FurnitureHelperContext context) : base(context) { }

        protected override void SingleInclude(AccountEntity entity)
        {
            if (entity == null)
                return;

            Context.Entry<AccountEntity>(entity).Collection(account => account.accounts_extensions).Load();
            Context.Entry<AccountEntity>(entity).Collection(account => account.ownings).Load();
            OwnershipRepo.ForEachIncludeCommon(Context, entity.ownings);
        }

        protected override void WholeInclude()
        {
            Context.accounts.Include(account => account.accounts_extensions)
                            .Include(account => account.ownings)
                            .Load();

            OwnershipRepo.WholeIncludeCommon(Context);
        }

        public AccountModel GetByLogin(string login)
        {
            AccountEntity accountEntity = Context.accounts.FirstOrDefault(account => account.login == login);
            SingleInclude(accountEntity);

            return accountEntity == null ? null : Mapper.Map<AccountEntity, AccountModel>(accountEntity);
        }

        public AccountModel GetByEmail(string email)
        {
            AccountEntity accountEntity = Context.accounts.FirstOrDefault(account => account.email == email);
            SingleInclude(accountEntity);

            return accountEntity == null ? null : Mapper.Map<AccountEntity, AccountModel>(accountEntity);
        }

        public AccountModel GetByOwnedPartMac(string mac)
        {
            ConcretePartEntity part = Context.concrete_parts.FirstOrDefault(p => p.controller_mac == mac);

            if (part == null)
                return null;

            AccountEntity account = part.ownings.LastOrDefault()?.accounts;
            return account == null ? null : Mapper.Map<AccountEntity, AccountModel>(account);
        }
    }
}
