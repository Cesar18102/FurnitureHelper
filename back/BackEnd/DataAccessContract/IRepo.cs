using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IRepo<TModel> where TModel : IModel
    {
        TModel Create(TModel model);
        TModel Update(int id, TModel model);
        TModel Delete(int id);
        TModel Get(int id);
        IEnumerable<TModel> GetAll();
    }
}