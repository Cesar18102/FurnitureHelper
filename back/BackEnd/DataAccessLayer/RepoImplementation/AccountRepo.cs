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

        protected override void ConfigEntityModelMapper(IMapperConfigurationExpression config)
        {
            config.ReplaceMemberName("accounts_extensions", "AccountExtensions");
            config.ReplaceMemberName("first_name", "FirstName");
            config.ReplaceMemberName("last_name", "LastName");
            config.ReplaceMemberName("pwd", "Password");

            config.CreateMap<AccountModel, AccountEntity>()
                  .ForAllMembers(memberConfigExpression => memberConfigExpression.Condition((model, entity, member) => member != null));

            config.CreateMap<AccountEntity, AccountModel>();
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
