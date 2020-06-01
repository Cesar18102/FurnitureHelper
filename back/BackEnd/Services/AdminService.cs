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

        public bool IsAdmin(int accountId) => AdminRepo.IsAdmin(accountId);
        public bool IsSuperAdmin(int accountId) => SuperAdminRepo.IsSuperAdmin(accountId);

        public void CheckActiveAdmin(SessionDto dto)
        {
            SessionService.CheckSession(dto);

            if (!IsAdmin(dto.UserId.GetValueOrDefault()))
                throw new ForbiddenException("admin");
        }

        public void CheckActiveSuperAdmin(SessionDto dto)
        {
            SessionService.CheckSession(dto);

            if (!IsSuperAdmin(dto.UserId.GetValueOrDefault()))
                throw new ForbiddenException("super-admin");
        }

        public AdminModel AddAdmin(AddAdminDto dto)
        {
            return ProtectedExecute<AddAdminDto, AdminModel>(adminDto =>
            {
                CheckActiveSuperAdmin(adminDto.SuperAdminSession);

                if (AccountRepo.Get(adminDto.AccountId.GetValueOrDefault()) == null)
                    throw new NotFoundException("Account");

                if (IsAdmin(adminDto.AccountId.GetValueOrDefault()))
                    throw new ConflictException("admin account");

                AdminModel model = Mapper.Map<AddAdminDto, AdminModel>(adminDto);
                return AdminRepo.Create(model);
            }, dto);
        }

        public SuperAdminModel AddSuperAdmin(AddAdminDto dto)
        {
            return ProtectedExecute<AddAdminDto, SuperAdminModel>(adminDto =>
            {
                CheckActiveSuperAdmin(adminDto.SuperAdminSession);

                if (AccountRepo.Get(adminDto.AccountId.GetValueOrDefault()) == null)
                    throw new NotFoundException("Account");

                if (IsSuperAdmin(adminDto.AccountId.GetValueOrDefault()))
                    throw new ConflictException("super-admin account");

                AdminModel admin = AdminRepo.GetByAccountId(adminDto.AccountId.GetValueOrDefault());

                if (admin == null)
                {
                    AdminModel adminModel = Mapper.Map<AddAdminDto, AdminModel>(adminDto);
                    admin = AdminRepo.Create(adminModel);
                }

                SuperAdminModel model = Mapper.Map<AdminModel, SuperAdminModel>(admin);
                return SuperAdminRepo.Create(model);
            }, dto);
        }

        public AdminModel GetAdminByUserId(int id)
        {
            return AdminRepo.GetByAccountId(id);
        }

        public SuperAdminModel GetSuperAdminByUserId(int id)
        {
            AdminModel admin = GetAdminByUserId(id);
            return admin == null ? null : Mapper.Map<AdminModel, SuperAdminModel>(admin);
        }
    }
}
