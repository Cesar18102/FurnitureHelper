using Autofac;
using AutoMapper;

using Models;

using DataAccessHolder;
using DataAccessContract;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        private static readonly IAccountRepo AccountRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountRepo>();

        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly HashingService Hasher = ServiceDependencyHolder.ServicesDependencies.Resolve<HashingService>();

        public AccountModel SignUp(SignUpDto dto)
        {
            dto.Password = Hasher.GetHash(dto.Password);
            AccountModel account = Mapper.Map<SignUpDto, AccountModel>(dto);

            if (AccountRepo.GetByLogin(account.Login) != null)
                throw new ConflictException("Login");

            if (AccountRepo.GetByEmail(account.Email) != null)
                throw new ConflictException("Email");

            return ProtectedExecute<SignUpDto, AccountModel>(model => AccountRepo.Create(model), account);
        }

        public SessionModel LogIn(LogInDto dto)
        {
            AccountModel account = AccountRepo.GetByLogin(dto.Login);

            if (account == null)
                throw new NotFoundException("Login");

            string originalPasswordSalted = Hasher.GetHash(account.Password + dto.Salt);
            if (originalPasswordSalted.ToUpper() != dto.PasswordSalted.ToUpper())
                throw new UnauthorizedException("Wrong password");

            return SessionService.CreateSessionFor(account.Id);
        }

        public AccountModel Update(UpdateAccountDto dto)
        {
            SessionService.CheckSession(dto.Session);

            if (dto.Id != dto.Session.UserId)
                throw new ForbiddenException("Account owner");

            dto.Password = Hasher.GetHash(dto.Password);
            AccountModel account = Mapper.Map<UpdateAccountDto, AccountModel>(dto);

            return ProtectedExecute<UpdateAccountDto, AccountModel>(
                model => AccountRepo.Update(model.Id, model), 
                account
            );
        }
    }
}
