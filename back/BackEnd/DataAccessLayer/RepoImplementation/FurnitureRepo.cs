using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

using Models;
using DataAccessContract;
using DataAccess.Entities;
using DataAccessContract.Exceptions;

namespace DataAccess.RepoImplementation
{
    public class FurnitureRepo : RepoBase<FurnitureItemModel, FurnitureItemEntity>, IFurnitureRepo
    {
        public FurnitureRepo(FurnitureHelperContext context) : base(context) { }

        protected override void SingleInclude(FurnitureItemEntity entity)
        {
            Context.Entry<FurnitureItemEntity>(entity).Collection(furniture => furniture.furniture_item_parts_connections).Load();
            Context.Entry<FurnitureItemEntity>(entity).Collection(furniture => furniture.used_parts).Load();

            foreach (FurnitureItemPartsConnectionEntity globalConnection in entity.furniture_item_parts_connections)
            {
                Context.Entry<FurnitureItemPartsConnectionEntity>(globalConnection).Collection(connection => connection.parts_connection_glues).Load();
                Context.Entry<FurnitureItemPartsConnectionEntity>(globalConnection).Collection(connection => connection.two_parts_connection).Load();

                foreach (PartsConnectionGlueEntity globalGlue in globalConnection.parts_connection_glues)
                    Context.Entry<PartsConnectionGlueEntity>(globalGlue).Reference(glue => glue.parts).Load();

                foreach (TwoPartsConnectionEntity twoPartConnection in globalConnection.two_parts_connection)
                {
                    DbEntityEntry<TwoPartsConnectionEntity> twoPartsConnectionEntry = Context.Entry<TwoPartsConnectionEntity>(twoPartConnection);

                    twoPartsConnectionEntry.Reference(connection => connection.part_controllers_embed_relative_positions).Load();
                    twoPartsConnectionEntry.Reference(connection => connection.part_controllers_embed_relative_positions1).Load();
                    twoPartsConnectionEntry.Collection(connection => connection.two_parts_connection_glues).Load();

                    foreach (TwoPartsConnectionGlueEntity twoPartsGlue in twoPartConnection.two_parts_connection_glues)
                        Context.Entry<TwoPartsConnectionGlueEntity>(twoPartsGlue).Reference(glue => glue.parts).Load();
                }
            }
        }

        private void RemoveAttachedConnections(FurnitureItemEntity entity)
        {
            foreach (FurnitureItemPartsConnectionEntity globalConnection in entity.furniture_item_parts_connections)
            {
                Context.parts_connection_glues.RemoveRange(globalConnection.parts_connection_glues);
                foreach (TwoPartsConnectionEntity connection in globalConnection.two_parts_connection)
                    Context.two_parts_connection_glues.RemoveRange(connection.two_parts_connection_glues);
                Context.two_parts_connection.RemoveRange(globalConnection.two_parts_connection);
            }
        }

        private void RemoveAttached(FurnitureItemEntity entity)
        {
            RemoveAttachedConnections(entity);
            Context.furniture_item_parts_connections.RemoveRange(entity.furniture_item_parts_connections);
            Context.used_parts.RemoveRange(entity.used_parts);
        }

        protected override void WholeInclude()
        {
            Context.furniture_items.Include(furniture => furniture.furniture_item_parts_connections);
            Context.furniture_items.Include(furniture => furniture.used_parts);
            Context.furniture_item_parts_connections.Include(connection => connection.parts_connection_glues)
                                                    .Include(connection => connection.two_parts_connection);
            Context.parts_connection_glues.Include(glue => glue.parts);
            Context.two_parts_connection.Include(connection => connection.part_controllers_embed_relative_positions)
                                        .Include(connection => connection.part_controllers_embed_relative_positions1)
                                        .Include(connection => connection.two_parts_connection_glues);
            Context.two_parts_connection_glues.Include(glue => glue.parts);
        }

        public FurnitureItemModel Update(int id, FurnitureItemModel model, IEnumerable<int> usedPartsToRemove, IEnumerable<UsedPartModel> usedPartsToAdd)
        {
            FurnitureItemEntity entity = Context.furniture_items.FirstOrDefault(furniture => furniture.id == id);

            if (entity == null)
                throw new EntityNotFoundException("furniture item");

            IEnumerable<UsedPartEntity> partsToDelete = entity.used_parts.Where(part => usedPartsToRemove.Contains(part.id)).ToList();
            foreach (UsedPartEntity deletedEntity in partsToDelete)
                Context.Entry<UsedPartEntity>(deletedEntity).State = EntityState.Deleted;

            IEnumerable<UsedPartEntity> usedPartToAddEntities = Mapper.Map<IEnumerable<UsedPartModel>, IEnumerable<UsedPartEntity>>(usedPartsToAdd);
            foreach (UsedPartEntity usedPart in usedPartToAddEntities)
            {
                entity.used_parts.Add(usedPart);
                Context.Entry<UsedPartEntity>(usedPart).State = EntityState.Added;
            }

            Context.SaveChanges();

            return Mapper.Map<FurnitureItemEntity, FurnitureItemModel>(entity);
        }

        public FurnitureItemModel UpdateConnections(int id, IEnumerable<GlobalPartsConnectionModel> connections)
        {
            FurnitureItemEntity entity = Context.furniture_items.FirstOrDefault(furniture => furniture.id == id);

            CheckValid(entity, connections);
            RemoveAttachedConnections(entity);

            entity.furniture_item_parts_connections = Mapper.Map<IEnumerable<GlobalPartsConnectionModel>, ICollection<FurnitureItemPartsConnectionEntity>>(connections);
            Context.SaveChanges();

            return Mapper.Map<FurnitureItemEntity, FurnitureItemModel>(entity);
        }

        private void CheckValid(FurnitureItemEntity entity, IEnumerable<GlobalPartsConnectionModel> connections)
        {
            foreach (GlobalPartsConnectionModel connection in connections)
            {
                foreach (ConnectionGlueModel glue in connection.GlobalConnectionGlues)
                    if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                        throw new EntityNotFoundException("global glue part");

                foreach (TwoPartsConnectionModel subConnection in connection.SubConnections)
                {
                    UsedPartEntity usedPart = entity.used_parts.FirstOrDefault(used => used.id == subConnection.UsedPartId);

                    if (usedPart == null)
                        throw new EntityNotFoundException("used part");

                    if(usedPart.parts.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ConnectionHelper.Id) == null)
                        throw new EntityNotFoundException("connection helper");


                    UsedPartEntity usedPartOther = entity.used_parts.FirstOrDefault(used => used.id == subConnection.UsedPartOtherId);

                    if (usedPartOther == null)
                        throw new EntityNotFoundException("used part other");

                    if (usedPartOther.parts.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ConnectionHelperOther.Id) == null)
                        throw new EntityNotFoundException("connection helper other");

                    foreach (ConnectionGlueModel glue in subConnection.ConnectionGlues)
                        if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                            throw new EntityNotFoundException("subconnection glue part");
                }
            }
        }
    }
}
