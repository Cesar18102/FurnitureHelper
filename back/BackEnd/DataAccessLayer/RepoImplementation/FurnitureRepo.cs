using System;
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

        public override FurnitureItemModel Create(FurnitureItemModel model)
        {
            CheckValidUsedPartsToAdd(model.UsedParts);
            return base.Create(model);
        }

        public override FurnitureItemModel Update(int id, FurnitureItemModel model)
        {
            throw new NotSupportedException();
        }

        public FurnitureItemModel Update(int id, FurnitureItemModel model, IEnumerable<int> usedPartsToRemove, IEnumerable<UsedPartModel> usedPartsToAdd)
        {
            FurnitureItemEntity entity = Context.furniture_items.FirstOrDefault(furniture => furniture.id == id);

            if (entity == null)
                throw new EntityNotFoundException("furniture item");

            IEnumerable<UsedPartEntity> partsToDelete = entity.used_parts.Where(part => usedPartsToRemove.Contains(part.id)).ToList();
            foreach (UsedPartEntity deletedEntity in partsToDelete)
                Context.Entry<UsedPartEntity>(deletedEntity).State = EntityState.Deleted;

            CheckValidUsedPartsToAdd(usedPartsToAdd);
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

        private void CheckValidUsedPartsToAdd(IEnumerable<UsedPartModel> usedParts)
        {
            foreach (UsedPartModel usedPart in usedParts)
                if (Context.parts.FirstOrDefault(part => part.id == usedPart.PartId) == null)
                    throw new EntityNotFoundException("part");
        }

        private void CheckValid(ConnectionGlueModel glue, Dictionary<int, int> usedParts, bool isGlobal)
        {
            string globalText = isGlobal ? "global " : "";
            if (!usedParts.ContainsKey(glue.GluePart.Id) || usedParts[glue.GluePart.Id] == 0)
                throw new EntityNotFoundException($"used part for {globalText}glue part");
            --usedParts[glue.GluePart.Id];
        }

        private void CheckValid(FurnitureItemEntity furnitureItem, TwoPartsConnectionModel subConnection, Dictionary<int, int> usedParts, List<int> mentioned)
        {
            UsedPartEntity part = furnitureItem.used_parts.FirstOrDefault(p => p.id == subConnection.UsedPartId);

            if (part == null || !part.part_id.HasValue || !usedParts.ContainsKey(part.part_id.Value) || usedParts[part.part_id.Value] == 0)
                throw new EntityNotFoundException($"used part for connection part {part.id}");

            if(part.parts.part_controllers_embed_relative_positions.FirstOrDefault(helper => helper.id == subConnection.ConnectionHelper.Id) == null)
                throw new EntityNotFoundException($"connection helper {subConnection.ConnectionHelper.Id}");

            if (!mentioned.Contains(subConnection.UsedPartId))
            {
                --usedParts[part.part_id.Value];
                mentioned.Add(subConnection.UsedPartId);
            }


            UsedPartEntity partOther = furnitureItem.used_parts.FirstOrDefault(p => p.id == subConnection.UsedPartOtherId);

            if (partOther == null || !partOther.part_id.HasValue || !usedParts.ContainsKey(partOther.part_id.Value) || usedParts[partOther.part_id.Value] == 0)
                throw new EntityNotFoundException($"used part for connection part {partOther.id}");

            if (partOther.parts.part_controllers_embed_relative_positions.FirstOrDefault(helper => helper.id == subConnection.ConnectionHelperOther.Id) == null)
                throw new EntityNotFoundException($"connection helper {subConnection.ConnectionHelperOther.Id}");

            if (!mentioned.Contains(subConnection.UsedPartOtherId))
            {
                --usedParts[partOther.part_id.Value];
                mentioned.Add(subConnection.UsedPartOtherId);
            }
        }

        private void CheckValid(FurnitureItemEntity entity, IEnumerable<GlobalPartsConnectionModel> connections)
        {
            Dictionary<int, int> usedParts = entity.used_parts.Where(part => part.part_id.HasValue)
                                                              .GroupBy(part => part.part_id.Value)
                                                              .ToDictionary(group => group.Key, group => group.Count());
            List<int> mentioned = new List<int>();

            foreach (GlobalPartsConnectionModel connection in connections)
            {
                foreach (ConnectionGlueModel glue in connection.GlobalConnectionGlues)
                    CheckValid(glue, usedParts, true);

                foreach (TwoPartsConnectionModel subConnection in connection.SubConnections)
                {
                    CheckValid(entity, subConnection, usedParts, mentioned);
                    foreach (ConnectionGlueModel glue in subConnection.ConnectionGlues)
                        CheckValid(glue, usedParts, false);
                }
            }
        }
    }
}
