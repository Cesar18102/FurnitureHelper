using Autofac;
using AutoMapper;

using DataAccessHolder;
using DataAccessContract;

using Models;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class AdminService : ServiceBase, IAdminService
    {
        private static readonly IAccountRepo AccountRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountRepo>();
        private static readonly ISuperAdminRepo SuperAdminRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<ISuperAdminRepo>();
        private static readonly IAdminRepo AdminRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAdminRepo>();

        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();

        public bool IsAdmin(int accountId)
        {
            return AdminRepo.IsAdmin(accountId);
        }

        public bool IsSuperAdmin(int accountId)
        {
            return SuperAdminRepo.IsSuperAdmin(accountId);
        }

        public void CheckActiveAdmin(SessionDto dto)
        {
            SessionService.CheckSession(dto);

            if (!IsAdmin(dto.UserId))
                throw new ForbiddenException("admin");
        }

        public void CheckActiveSuperAdmin(SessionDto dto)
        {
            SessionService.CheckSession(dto);

            if (!IsSuperAdmin(dto.UserId))
                throw new ForbiddenException("super-admin");
        }

        public AdminModel AddAdmin(AddAdminDto dto)
        {
            CheckActiveSuperAdmin(dto.SuperAdminSession);

            if (AccountRepo.Get(dto.AccountId) == null)
                throw new NotFoundException("Account");

            if (IsAdmin(dto.AccountId))
                throw new ConflictException("admin account");

            AdminModel model = Mapper.Map<AddAdminDto, AdminModel>(dto);
            return AdminRepo.Create(model);
        }

        public SuperAdminModel AddSuperAdmin(AddAdminDto dto)
        {
            CheckActiveSuperAdmin(dto.SuperAdminSession);

            if (AccountRepo.Get(dto.AccountId) == null)
                throw new NotFoundException("Account");

            if (IsSuperAdmin(dto.AccountId))
                throw new ConflictException("super-admin account");

            AdminModel admin = AdminRepo.GetByAccountId(dto.AccountId);

            if (admin == null)
            {
                AdminModel adminModel = Mapper.Map<AddAdminDto, AdminModel>(dto);
                admin = AdminRepo.Create(adminModel);
            }

            SuperAdminModel model = Mapper.Map<AdminModel, SuperAdminModel>(admin);
            return SuperAdminRepo.Create(model);
        }
    }
}
