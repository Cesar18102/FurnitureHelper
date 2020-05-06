using System.Collections.Generic;

using Models;

namespace DataAccessContract
{
    public interface IMaterialRepo : IRepo<MaterialModel>
    {
        MaterialModel GetByName(string name);
    }
}