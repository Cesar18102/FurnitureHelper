using System.Linq;
using System.Collections.Generic;

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
        private static readonly IPartRepo PartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IPartRepo>();

        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly AdminService AdminService = ServiceDependencyHolder.ServicesDependencies.Resolve<AdminService>();
        private static readonly IPartService PartService = ServiceDependencyHolder.ServicesDependencies.Resolve<IPartService>();

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

        public InvariantPartStore GetPartStore(int furnitureItemId)
        {
            FurnitureItemModel furniture = FurnitureRepo.Get(furnitureItemId);

            if (furniture == null)
                throw new NotFoundException("furniture");

            IEnumerable<IGrouping<int, UsedPartModel>> usedParts = furniture.UsedParts.Where(part => part.PartId.HasValue)
                                                                                      .GroupBy(part => part.PartId.Value);

            InvariantPartStore store = new InvariantPartStore();
            foreach (IGrouping<int, UsedPartModel> used in usedParts)
            {
                PartModel part = PartRepo.Get(used.Key);
                InvariantPartStorePosition position = new InvariantPartStorePosition(part, used.Count());
                store.Positions.Add(position);
            }
            return store;
        }

        public bool CanBuild(SessionDto session, int furnitureItemId)
        {
            SessionService.CheckSession(session);
            InvariantPartStore itemPartStore = GetPartStore(furnitureItemId);
            InvariantPartStore userPartStore = PartService.GetOwnedInvariant(session);
            return userPartStore.Contains(itemPartStore);
        }

        public IEnumerable<FurnitureItemModel> GetBuildList(SessionDto session)
        {
            return ProtectedExecute<SessionDto, FurnitureItemModel>(sessionDto =>
            {
                SessionService.CheckSession(session);

                Dictionary<FurnitureItemModel, InvariantPartStore> partStores = GetAll().ToDictionary(
                    furniture => furniture, 
                    furniture => GetPartStore(furniture.Id)
                );

                InvariantPartStore userPartStore = PartService.GetOwnedInvariant(sessionDto);
                return partStores.Where(store => userPartStore.Contains(store.Value))
                                 .Select(store => store.Key);

            }, session);
        }

        public IEnumerable<FurnitureItemModel> GetAll()
        {
            return FurnitureRepo.GetAll();
        }
    }
}
