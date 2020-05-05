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

        protected override void SingleInclude(MaterialEntity entity)
        {
            Context.Entry<MaterialEntity>(entity).Collection(material => material.colors).Load();
        }

        protected override void WholeInclude()
        {
            Context.materials.Include(material => material.colors).Load();
        }

        public override MaterialModel Create(MaterialModel model)
        {
            MaterialModel material = base.Create(model);
            return UpdateAttachedColors(material, model.PossibleColors);
        }

        public override MaterialModel Update(int id, MaterialModel model)
        {
            MaterialModel material = base.Update(id, model);

            if (material == null)
                return null;

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
            return found == null ? null : Mapper.Map<MaterialEntity, MaterialModel>(found);
        }
    }
}
