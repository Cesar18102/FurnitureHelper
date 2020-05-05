using System.Linq;

using AutoMapper;

using Models;
using DataAccessContract;
using DataAccess.Entities;

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
    }
}
