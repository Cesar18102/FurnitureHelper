using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IAdminService
    {
        AdminModel AddAdmin(AddAdminDto dto);
        SuperAdminModel AddSuperAdmin(AddAdminDto dto);

        void CheckActiveAdmin(SessionDto dto);
        void CheckActiveSuperAdmin(SessionDto dto);

        AdminModel GetAdminByUserId(int id);
        SuperAdminModel GetSuperAdminByUserId(int id);
    }
}
