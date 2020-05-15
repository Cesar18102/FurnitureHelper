using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Models;

using DataAccessContract;
using DataAccess.Entities;
using DataAccessContract.Exceptions;
using System;

namespace DataAccess.RepoImplementation
{
    public class PartRepo : RepoBase<PartModel, PartEntity>, IPartRepo
    {
        public PartRepo(FurnitureHelperContext context) : base(context) { }

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

            Context.part_controllers_embed_relative_positions.RemoveRange(entity.part_controllers_embed_relative_positions);

            PartModel updatedPart = base.Update(id, model);
            return UpdateAttachedMaterials(id, model.PossibleMaterials);
        }

        protected override void SingleInclude(PartEntity entity)
        {
            Context.Entry<PartEntity>(entity).Collection(part => part.materials).Load();
            Context.Entry<PartEntity>(entity).Collection(part => part.part_controllers_embed_relative_positions).Load();
        }

        protected override void WholeInclude()
        {
            Context.parts.Include(part => part.materials)
                         .Include(part => part.part_controllers_embed_relative_positions)
                         .Load();
        }

        public PartModel UpdateAttachedMaterials(int partId, IEnumerable<MaterialModel> materials)
        {
            PartEntity entity = Context.parts.FirstOrDefault(part => part.id == partId);
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
    }
}
