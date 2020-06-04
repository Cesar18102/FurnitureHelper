using System.Collections.Generic;

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
            return ProtectedExecute<AddMaterialDto, MaterialModel>(materialDto => {
                AdminService.CheckActiveSuperAdmin(materialDto.SuperAdminSession);
                MaterialModel model = Mapper.Map<AddMaterialDto, MaterialModel>(materialDto);

                if (MaterialRepo.GetByName(model.Name) != null)
                    throw new ConflictException("Material name");

                return MaterialRepo.Create(model);
            }, dto);
        }

        public MaterialModel UpdateMaterial(UpdateMaterialDto dto)
        {
            return ProtectedExecute<UpdateMaterialDto, MaterialModel>(materialDto => {
                AdminService.CheckActiveSuperAdmin(materialDto.SuperAdminSession);
                MaterialModel model = Mapper.Map<UpdateMaterialDto, MaterialModel>(materialDto);
                MaterialModel foundMaterial = MaterialRepo.GetByName(model.Name);

                if (foundMaterial != null && foundMaterial.Id != dto.Id)
                    throw new ConflictException("Material name");

                return MaterialRepo.Update(model.Id, model);
            }, dto);
        }

        public MaterialModel DeleteMaterial(DeleteDto dto)
        {
            return ProtectedExecute<DeleteDto, MaterialModel>(deleteDto =>
            {
                AdminService.CheckActiveSuperAdmin(deleteDto.Session);

                if (MaterialRepo.HasAttachedPart(dto.DeletedId.Value))
                    throw new ConflictException("attached part");

                return MaterialRepo.Delete(dto.DeletedId.Value);
            }, dto);
        }

        public IEnumerable<MaterialModel> GetAll()
        {
            return MaterialRepo.GetAll();
        }

        public MaterialModel Get(int materialId)
        {
            return MaterialRepo.Get(materialId);
        }
    }
}
