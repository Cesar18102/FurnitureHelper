using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using Models;
using DataAccessContract;
using DataAccess.Entities;

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

                    foreach (TwoPartsConnectionGlueEntity twoPartsGlue in twoPartConnection.two_parts_connection_glues)
                        Context.Entry<TwoPartsConnectionGlueEntity>(twoPartsGlue).Reference(glue => glue.parts).Load();
                }

                foreach (PartsConnectionGlueEntity partsGlue in globalConnection.parts_connection_glues)
                    Context.Entry<PartsConnectionGlueEntity>(partsGlue).Reference(glue => glue.parts).Load();
            }
        }

        protected override void WholeInclude()
        {
            Context.furniture_items.Include(furniture => furniture.furniture_item_parts_connections);
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
            return IsValid(model) ? base.Create(model) : null;
        }

        public override FurnitureItemModel Update(int id, FurnitureItemModel model)
        {
            return IsValid(model) ? base.Update(id, model) : null;
        }

        private bool IsValid(FurnitureItemModel model)
        {
            foreach (GlobalPartsConnectionModel connection in model.GlobalConnections)
            {
                foreach (ConnectionGlueModel glue in connection.GlobalConnectionGlues)
                    if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                        return false;

                foreach(TwoPartsConnectionModel subConnection in connection.SubConnections)
                {
                    if (Context.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ControllerPosition.Id) == null)
                        return false;

                    if (Context.part_controllers_embed_relative_positions.FirstOrDefault(position => position.id == subConnection.ControllerPositionOther.Id) == null)
                        return false;

                    foreach (ConnectionGlueModel glue in subConnection.ConnectionGlues)
                        if (Context.parts.FirstOrDefault(part => part.id == glue.GluePart.Id) == null)
                            return false;
                }
            }

            return true;
        }
    }
}
