using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IAdminService
    {
        AdminModel AddAdmin(AddAdminDto dto);
        SuperAdminModel AddSuperAdmin(AddAdminDto dto);
    }
}
