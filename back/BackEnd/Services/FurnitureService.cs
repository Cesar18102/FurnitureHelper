using Autofac;

using Models;
using DataAccessContract;
using DataAccessHolder;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class FurnitureService : ServiceBase, IFurnitureService
    {
        private static readonly IFurnitureRepo FurnitureRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IFurnitureRepo>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();

        public FurnitureItemModel RegisterFurnitureItem(AddFurnitureItemDto dto)
        {
            FurnitureItemModel model = Mapper.Map<AddFurnitureItemDto, FurnitureItemModel>(dto);

            FurnitureItemModel created = ProtectedExecute<AddFurnitureItemDto, FurnitureItemModel>(
                furniture => FurnitureRepo.Create(furniture),
                model
            );

            if (created == null)
                throw new NotFoundException("glue part or controller position");

            return created;
        }

        /*public FurnitureItemModel UpdateFurnitureItem(AddFurnitureItemDto dto)
        {
            return null;
        }*/
    }
}
