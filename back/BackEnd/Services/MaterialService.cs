using Autofac;

using Models;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

using DataAccessHolder;
using DataAccessContract;

namespace Services
{
    public class MaterialService : ServiceBase, IMaterialService
    {
        private static readonly IMaterialRepo MaterialRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IMaterialRepo>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public MaterialModel RegisterMaterial(AddMaterialDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            MaterialModel model = Mapper.Map<AddMaterialDto, MaterialModel>(dto);

            if (MaterialRepo.GetByName(model.Name) != null)
                throw new ConflictException("Material name");

            return ProtectedExecute<AddMaterialDto, MaterialModel>(material => MaterialRepo.Create(material), model);
        }

        public MaterialModel UpdateMaterial(UpdateMaterialDto dto)
        {
            AdminService.CheckActiveSuperAdmin(dto.SuperAdminSession);
            MaterialModel model = Mapper.Map<UpdateMaterialDto, MaterialModel>(dto);
            MaterialModel foundMaterial = MaterialRepo.GetByName(model.Name);

            if (foundMaterial != null && foundMaterial.Id != dto.Id)
                throw new ConflictException("Material name");

            return ProtectedExecute<UpdateMaterialDto, MaterialModel>(
                material => MaterialRepo.Update(material.Id, material), 
                model
            );
        }
    }
}
