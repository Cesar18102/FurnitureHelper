using Autofac;

using Models;
using DataAccessHolder;
using DataAccessContract;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class PartService : ServiceBase, IPartService
    {
        private static readonly IPartRepo PartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IPartRepo>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public PartModel RegisterPart(AddPartDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            PartModel model = Mapper.Map<AddPartDto, PartModel>(dto);
            return ProtectedExecute<AddPartDto, PartModel>(part => PartRepo.Create(part), model);
        }

        public PartModel UpdatePart(UpdatePartDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            PartModel model = Mapper.Map<UpdatePartDto, PartModel>(dto);

            return ProtectedExecute<UpdatePartDto, PartModel>(
                part => PartRepo.Update(part.Id, part), 
                model
            );
        }
    }
}
