using Autofac;

using Models;
using DataAccessContract;
using DataAccessHolder;

using ServicesContract;
using ServicesContract.Dto;

namespace Services
{
    public class FurnitureService : ServiceBase, IFurnitureService
    {
        private static readonly IFurnitureRepo FurnitureRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IFurnitureRepo>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public FurnitureItemModel RegisterFurnitureItem(AddFurnitureDto dto)
        {
            AdminService.CheckActiveAdmin(dto.AdminSession);
            FurnitureItemModel model = Mapper.Map<AddFurnitureDto, FurnitureItemModel>(dto);

            return ProtectedExecute<AddFurnitureDto, FurnitureItemModel>(
                furniture => FurnitureRepo.Create(furniture),
                model
            );
        }

        public FurnitureItemModel UpdateFurnitureItem(UpdateFurnitureDto dto)
        {
            AdminService.CheckActiveAdmin(dto.AdminSession);
            FurnitureItemModel model = Mapper.Map<UpdateFurnitureDto, FurnitureItemModel>(dto);

            return ProtectedExecute<UpdateFurnitureDto, FurnitureItemModel>(
                furniture => FurnitureRepo.Update(furniture.Id, furniture),
                model
            );
        }
    }
}
