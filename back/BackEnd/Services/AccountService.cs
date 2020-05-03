using Autofac;

using DataTypes.Dto;
using DataTypes.Models;
using DataTypes.Exceptions;

using DataAccessHolder;
using DataAccessContract;

using ServicesContract;

namespace Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepo AccountRepo = DataAccessDependencyHolder.DataAccessDependencies.Resolve<IAccountRepo>();

        public AccountModel Create(AccountDto accountDto)
        {
            if (AccountRepo.GetByLogin(accountDto.Login) != null)
                throw new ConftictException("Login");

            if(AccountRepo.GetByEmail(accountDto.Email) != null)
                throw new ConftictException("Email");

            return AccountRepo.Create(accountDto);
        }

        public AccountModel Update(int id, AccountDto accountDto)
        {
            AccountModel updatedAccount = AccountRepo.Update(id, accountDto);

            if (updatedAccount == null)
                throw new NotFoundException("Account");

            return updatedAccount;
        }
    }
}
