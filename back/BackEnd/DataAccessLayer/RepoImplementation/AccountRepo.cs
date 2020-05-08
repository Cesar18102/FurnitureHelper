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
            Context.Entry<AccountEntity>(entity).Collection(account => account.accounts_extensions).Load();
        }

        protected override void WholeInclude()
        {
            Context.accounts.Include(account => account.accounts_extensions).Load();
        }

        public AccountModel GetByLogin(string login)
        {
            AccountEntity accountEntity = Context.accounts.FirstOrDefault(account => account.login == login);
            return accountEntity == null ? null : Mapper.Map<AccountEntity, AccountModel>(accountEntity);
        }

        public AccountModel GetByEmail(string email)
        {
            AccountEntity accountEntity = Context.accounts.FirstOrDefault(account => account.email == email);
            return accountEntity == null ? null : Mapper.Map<AccountEntity, AccountModel>(accountEntity);
        }

        /*public AccountExtensionModel GetExtensionById(int id)
        {
            AccountExtensionEntity accountExtensionEntity = Context.accounts_extensions.FirstOrDefault(extension => extension.id == id);
            return accountExtensionEntity == null ? null : Mapper.Map<AccountExtensionEntity, AccountExtensionModel>(accountExtensionEntity);
        }

        public AccountModel AddAccountExtension(AccountExtensionModel accountExtension)
        {
            AccountEntity account = Context.accounts.FirstOrDefault(acc => acc.id == accountExtension.AccountId);

            if (account == null)
                throw new EntityNotFoundException("account");

            return ProtectedExecute(acc =>
            {
                AccountExtensionEntity extension = Mapper.Map<AccountExtensionModel, AccountExtensionEntity>(accountExtension);
                account.accounts_extensions.Add(extension);

                Context.SaveChanges();
                return Mapper.Map<AccountEntity, AccountModel>(account);
            }, account);
        }

        public AccountModel UpdateAccountExtensionLastUsedDate(int accountExtensionId, DateTime lastUsedDate)
        {
            AccountExtensionEntity accountExtension = Context.accounts_extensions.FirstOrDefault(ex => ex.id == accountExtensionId);

            if (accountExtension == null)
                throw new EntityNotFoundException("account extension");

            return ProtectedExecute(acc =>
            {
                accountExtension.last_used = lastUsedDate;
                Context.SaveChanges();
                return Mapper.Map<AccountEntity, AccountModel>(acc);
            }, accountExtension.accounts);
        }*/
    }
}
