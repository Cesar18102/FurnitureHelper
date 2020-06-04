using System.Linq;

using AutoMapper;

using Models;
using DataAccessContract;
using DataAccess.Entities;
using DataAccessContract.Exceptions;

namespace DataAccess.RepoImplementation
{
    public class ColorRepo : RepoBase<PartColorModel, PartColorEntity>, IColorRepo
    {
        public ColorRepo(FurnitureHelperContext context) : base(context) { }

        public PartColorModel GetByName(string name)
        {
            PartColorEntity colorEntity = Context.colors.FirstOrDefault(color => color.name == name);
            return colorEntity == null ? null : Mapper.Map<PartColorEntity, PartColorModel>(colorEntity);
        }

        public bool HasAttachedMaterial(int id)
        {
            PartColorEntity color = Context.colors.FirstOrDefault(clr => clr.id == id);

            if (color == null)
                throw new EntityNotFoundException("color");

            Context.Entry<PartColorEntity>(color).Collection(clr => clr.materials).Load();
            return color.materials.Count > 0;
        }
    }
}
