using System.Collections.Generic;

using DataTypes;

namespace DataAccessContract
{
    public interface IRepo<TDto, TModel> where TDto : IDto
                                         where TModel : IModel
    {
        TModel Create(TDto dto);
        TModel Update(int id, TDto dto);
        TModel Delete(int id);
        TModel Get(int id);
        IEnumerable<TModel> GetAll();
    }
}