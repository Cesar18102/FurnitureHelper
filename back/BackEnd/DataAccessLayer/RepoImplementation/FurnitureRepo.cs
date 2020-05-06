using System.Linq;
using System.Data.Entity;
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

            foreach (FurnitureItemPartsConnectionEntity globalConnection in entity.furniture_item_parts_connections)
            {
                Context.Entry<FurnitureItemPartsConnectionEntity>(globalConnection).Collection(connection => connection.parts_connection_glues).Load();
                Context.Entry<FurnitureItemPartsConnectionEntity>(globalConnection).Collection(connection => connection.two_parts_connection).Load();

                foreach (TwoPartsConnectionEntity twoPartConnection in globalConnection.two_parts_connection)
                {
                    DbEntityEntry<TwoPartsConnectionEntity> twoPartsConnectionEntry = Context.Entry<TwoPartsConnectionEntity>(twoPartConnection);

                    twoPartsConnectionEntry.Reference(connection => connection.part_controllers_embed_relative_positions).Load();
                    twoPartsConnectionEntry.Reference(connection => connection.part_controllers_embed_relative_positions1).Load();
                    twoPartsConnectionEntry.Collection(connection => connection.two_parts_connection_glues).Load();
                }
            }
        }

        private void RemoveAttached(FurnitureItemEntity entity)
        {
            foreach (FurnitureItemPartsConnectionEntity globalConnection in entity.furniture_item_parts_connections)
            {
                Context.parts_connection_glues.RemoveRange(globalConnection.parts_connection_glues);
                foreach (TwoPartsConnectionEntity connection in globalConnection.two_parts_connection)
                    Context.two_parts_connection_glues.RemoveRange(connection.two_parts_connection_glues);
                Context.two_parts_connection.RemoveRange(globalConnection.two_parts_connection);
            }

            Context.furniture_item_parts_connections.RemoveRange(entity.furniture_item_parts_connections);
        }

        protected override void WholeInclude()
        {
            Context.furniture_items.Include(furniture => furniture.furniture_item_parts_connections);
            Context.furniture_item_parts_connections.Include(connection => connection.parts_connection_glues)
                                                    .Include(connection => connection.two_parts_connection);
            Context.two_parts_connection.Include(connection => connection.part_controllers_embed_relative_positions)
                                        .Include(connection => connection.part_controllers_embed_relative_positions1)
                                        .Include(connection => connection.two_parts_connection_glues);
        }

        public override FurnitureItemModel Create(FurnitureItemModel model)
        {
            CheckValid(model);
            return base.Create(model);
        }

        public override FurnitureItemModel Update(int id, FurnitureItemModel model)
        {
            FurnitureItemEntity entity = Context.furniture_items.FirstOrDefault(furniture => furniture.id == id);

            if(entity == null)
                throw new EntityNotFoundException("furniture item");

            CheckValid(model);
            RemoveAttached(entity);

            return base.Update(id, model);
        }

        private void CheckValid(FurnitureItemModel model)
        {
            foreach (GlobalPartsConnectionModel connection in model.GlobalConnections)
            {
                foreach (ConnectionGlueModel glue in connection.GlobalConnectionGlues)
                    if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                        throw new EntityNotFoundException("global glue part");

                foreach (TwoPartsConnectionModel subConnection in connection.SubConnections)
                {
                    if (Context.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ControllerPosition.Id) == null)
                        throw new EntityNotFoundException("controller position");

                    if (Context.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ControllerPositionOther.Id) == null)
                        throw new EntityNotFoundException("controller position");

                    foreach (ConnectionGlueModel glue in subConnection.ConnectionGlues)
                        if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                            throw new EntityNotFoundException("subconnection glue part");
                }
            }
        }
    }
}
