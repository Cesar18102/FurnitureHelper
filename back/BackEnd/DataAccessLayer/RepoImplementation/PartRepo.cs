using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Models;

using DataAccess.Entities;
using DataAccessContract;
using DataAccessContract.Exceptions;

namespace DataAccess.RepoImplementation
{
    public class PartRepo : RepoBase<PartModel, PartEntity>, IPartRepo
    {
        public PartRepo(FurnitureHelperContext context) : base(context) { }

        public static void SingleIncludeCommon(FurnitureHelperContext context, PartEntity entity)
        {
            if (entity == null)
                return;

            context.Entry<PartEntity>(entity).Collection(part => part.materials).Load();
            MaterialRepo.ForEachIncludeCommon(context, entity.materials);
            context.Entry<PartEntity>(entity).Collection(part => part.part_controllers_embed_relative_positions).Load();
        }

        public static void WholeIncludeCommon(FurnitureHelperContext context)
        {
            context.parts.Include(part => part.materials)
                         .Include(part => part.part_controllers_embed_relative_positions)
                         .Load();

            MaterialRepo.WholeIncludeCommon(context);
        }

        public static void ForEachIncludeCommon(FurnitureHelperContext context, IEnumerable<PartEntity> entities)
        {
            foreach (PartEntity entity in entities)
                SingleIncludeCommon(context, entity);
        }

        protected override void SingleInclude(PartEntity entity) => SingleIncludeCommon(Context, entity);
        protected override void WholeInclude() => WholeIncludeCommon(Context);

        public override PartModel Create(PartModel model)
        {
            PartModel part = base.Create(model);
            return UpdateAttachedMaterials(part.Id, model.PossibleMaterials);
        }

        public override PartModel Update(int id, PartModel model)
        {
            PartEntity entity = Context.parts.FirstOrDefault(part => part.id == id);

            if (entity == null)
                throw new EntityNotFoundException("part");

            Context.Entry<PartEntity>(entity).Reload();
            IEnumerable<MaterialModel> materials = model.PossibleMaterials == null ? null : model.PossibleMaterials.ToList();

            if (entity.concrete_parts.Count == 0 && entity.used_parts.Count == 0)
            {
                if (model.ConnectionHelpers != null)
                {
                    IEnumerable<int> oldPositions = entity.part_controllers_embed_relative_positions.Select(pos => pos.id).ToList();
                    IEnumerable<int> updatedPositions = model.ConnectionHelpers.Where(helper => helper.Id.HasValue).Select(helper => helper.Id.Value).ToList();

                    IEnumerable<PartControllerEmbedRelativePositionEntity> removed = oldPositions.Except(updatedPositions)
                        .Select(removedId => entity.part_controllers_embed_relative_positions.FirstOrDefault(pos => pos.id == removedId))
                        .Where(pos => pos != null).ToList();

                    if (removed.Count() != 0)
                        Context.part_controllers_embed_relative_positions.RemoveRange(removed);

                    foreach (ConnectionHelperModel helper in model.ConnectionHelpers)
                    {
                        if (helper.Id.HasValue)
                        {
                            PartControllerEmbedRelativePositionEntity helperEntity = entity.part_controllers_embed_relative_positions
                                .First(pos => pos.id == helper.Id.Value);

                            Mapper.Map<ConnectionHelperModel, PartControllerEmbedRelativePositionEntity>(helper, helperEntity);
                        }
                        else
                        {
                            PartControllerEmbedRelativePositionEntity helperEntity =
                                Mapper.Map<ConnectionHelperModel, PartControllerEmbedRelativePositionEntity>(helper);

                            entity.part_controllers_embed_relative_positions.Add(helperEntity);
                        }
                    }
                }

                PartModel updatedPart = base.Update(id, model);
            }
            else
            {
                Mapper.Map<PartModel, PartEntity>(model, entity);
                Context.SaveChanges();
            }

            return UpdateAttachedMaterials(id, materials);
        }

        public PartModel UpdateAttachedMaterials(int partId, IEnumerable<MaterialModel> materials)
        {
            PartEntity entity = Context.parts.FirstOrDefault(part => part.id == partId);

            if (materials == null)
                return Mapper.Map<PartEntity, PartModel>(entity);

            entity.materials.Clear();

            foreach (MaterialModel material in materials)
            {
                MaterialEntity materialEntity = Context.materials.FirstOrDefault(mat => mat.id == material.Id);
                if (materialEntity != null)
                    entity.materials.Add(materialEntity);
            }

            return ProtectedExecute(part =>
            {
                Context.SaveChanges();
                return Mapper.Map<PartEntity, PartModel>(part);
            }, entity);
        }

        public bool WasBought(int id)
        {
            PartEntity entity = Context.parts.FirstOrDefault(part => part.id == id);

            if (entity == null)
                throw new EntityNotFoundException("part");

            return entity.concrete_parts.Count(cpart => cpart.last_sell_date != null) > 0;
        }
    }
}
