using System.Linq;
using System.Data.Entity;

using AutoMapper;

using DataTypes.Dto;
using DataTypes.Models;

using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class AccountRepo : RepoBase<AccountDto, AccountModel, AccountEntity>, IAccountRepo
    {
        public AccountRepo(FurnitureHelperContext context) : base(context) { }

        protected override IMappingExpression<AccountDto, AccountEntity> ConfigDtoEntityMapper(IMapperConfigurationExpression config)
        {
            config.ReplaceMemberName("Password", "pwd");
            return config.CreateMap<AccountDto, AccountEntity>();
        }

        protected override IMappingExpression<AccountEntity, AccountModel> ConfigEntityModelMapper(IMapperConfigurationExpression config)
        {
            config.ReplaceMemberName("accounts_extensions", "AccountExtensions");
            config.ReplaceMemberName("first_name", "FirstName");
            config.ReplaceMemberName("last_name", "LastName");
            config.ReplaceMemberName("pwd", "Password");
            return config.CreateMap<AccountEntity, AccountModel>();
        }

        protected override void SingleInclude(AccountEntity entity) => 
            Context.Entry<AccountEntity>(entity)
                   .Collection<AccountExtensionEntity>(account => account.accounts_extensions)
                   .Load();

        protected override void WholeInclude() => 
            Context.accounts.Include(account => account.accounts_extensions);

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
    }
}
