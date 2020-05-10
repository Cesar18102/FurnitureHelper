using System.Linq;
using System.Collections.Generic;

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
            return ProtectedExecute<AddFurnitureDto, FurnitureItemModel>(furnitureDto =>
            {
                AdminService.CheckActiveAdmin(furnitureDto.AdminSession);
                FurnitureItemModel model = Mapper.Map<AddFurnitureDto, FurnitureItemModel>(furnitureDto);
                return FurnitureRepo.Create(model);
            }, dto);
        }

        public FurnitureItemModel UpdateFurnitureItem(UpdateFurnitureDto dto)
        {
            return ProtectedExecute<UpdateFurnitureDto, FurnitureItemModel>(furnitureDto =>
            {
                AdminService.CheckActiveAdmin(furnitureDto.AdminSession);

                FurnitureItemModel model = Mapper.Map<UpdateFurnitureDto, FurnitureItemModel>(furnitureDto);
                IEnumerable<UsedPartModel> partsToAdd = furnitureDto.UsedPartsToAdd.Aggregate(
                    new List<UsedPartModel>(),
                    (acc, parts) => acc.Concat(Enumerable.Repeat(new UsedPartModel(parts.PartId.GetValueOrDefault()), parts.Count.GetValueOrDefault())).ToList()
                );

                return FurnitureRepo.Update(model.Id, model, furnitureDto.UsedPartsToRemove, partsToAdd);
            }, dto);
        }

        public FurnitureItemModel UpdateConnections(ConnectionsDto dto)
        {
            return ProtectedExecute<ConnectionsDto, FurnitureItemModel>(connDto =>
            {
                AdminService.CheckActiveAdmin(connDto.AdminSession);
                return FurnitureRepo.UpdateConnections(
                    connDto.FurnitureItemId.GetValueOrDefault(),
                    Mapper.Map<IEnumerable<GlobalConnectionDto>, IEnumerable<GlobalPartsConnectionModel>>(connDto.GlobalConnections)
                );
            }, dto);
        }

        public IEnumerable<FurnitureItemModel> GetAll()
        {
            return FurnitureRepo.GetAll();
        }
    }
}
