using System.Collections.Generic;

namespace DataContract
{
    public interface IRepo<T> where T : class
    {
        T Create(T item);
        T Update(T item);
        T Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
    }
}
