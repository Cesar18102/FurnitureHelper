using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using Models;
using DataAccessContract;
using DataAccess.Entities;

namespace DataAccess.RepoImplementation
{
    public class MaterialRepo : RepoBase<MaterialModel, MaterialEntity>, IMaterialRepo
    {
        public MaterialRepo(FurnitureHelperContext context) : base(context) { }

        public static void ForEachIncludeCommon(FurnitureHelperContext context, IEnumerable<MaterialEntity> entities)
        {
            foreach (MaterialEntity entity in entities)
                SingleIncludeCommon(context, entity);
        }

        public static void SingleIncludeCommon(FurnitureHelperContext context, MaterialEntity entity)
        {
            if (entity != null)
                context.Entry<MaterialEntity>(entity).Collection(material => material.colors).Load();
        }

        public static void WholeIncludeCommon(FurnitureHelperContext context)
        {
            context.materials.Include(material => material.colors).Load();
        }

        protected override void SingleInclude(MaterialEntity entity) => SingleIncludeCommon(Context, entity);
        protected override void WholeInclude() => WholeIncludeCommon(Context);

        public override MaterialModel Create(MaterialModel model)
        {
            MaterialModel material = base.Create(model);
            return UpdateAttachedColors(material, model.PossibleColors);
        }

        public override MaterialModel Update(int id, MaterialModel model)
        {
            MaterialModel material = base.Update(id, model);
            return UpdateAttachedColors(material, model.PossibleColors);
        }

        public MaterialModel UpdateAttachedColors(MaterialModel model, IEnumerable<PartColorModel> colors)
        {
            MaterialEntity entity = Context.materials.FirstOrDefault(material => material.id == model.Id);
            entity.colors.Clear();

            foreach (PartColorModel color in colors)
            {
                PartColorEntity colorEntity = Context.colors.FirstOrDefault(clr => clr.id == color.Id);
                if (colorEntity != null)
                    entity.colors.Add(colorEntity);
            }

            return ProtectedExecute(material =>
            {
                Context.SaveChanges();
                return Mapper.Map<MaterialEntity, MaterialModel>(material);
            }, entity);
        }

        public MaterialModel GetByName(string name)
        {
            MaterialEntity found = Context.materials.FirstOrDefault(material => material.name == name);
            SingleInclude(found);

            return found == null ? null : Mapper.Map<MaterialEntity, MaterialModel>(found);
        }
    }
}
