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
            return ProtectedExecute<SignUpDto, AccountModel>(accountDto =>
            {
                accountDto.Password = Hasher.GetHash(accountDto.Password);
                AccountModel model = Mapper.Map<SignUpDto, AccountModel>(accountDto);

                if (AccountRepo.GetByLogin(model.Login) != null)
                    throw new ConflictException("Login");

                if (AccountRepo.GetByEmail(model.Email) != null)
                    throw new ConflictException("Email");

                return AccountRepo.Create(model);
            }, dto);
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
            return ProtectedExecute<UpdateAccountDto, AccountModel>(accountDto =>
            {
                SessionService.CheckSession(accountDto.Session);

                if (accountDto.Id != accountDto.Session.UserId)
                    throw new ForbiddenException("Account owner");

                accountDto.Password = Hasher.GetHash(accountDto.Password);
                AccountModel model = Mapper.Map<UpdateAccountDto, AccountModel>(accountDto);

                return AccountRepo.Update(model.Id, model);
            }, dto);
        }

        public AccountModel GetInfo(SessionDto dto)
        {
            SessionService.CheckSession(dto);
            return AccountRepo.Get(dto.UserId.GetValueOrDefault());
        }
    }
}
