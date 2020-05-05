﻿using Models;

namespace DataAccessContract
{
    public interface IColorRepo : IRepo<PartColorModel>
    {
        PartColorModel GetByName(string name);
    }
}
