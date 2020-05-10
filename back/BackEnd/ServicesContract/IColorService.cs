using System.Collections.Generic;

using Models;

using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IColorService
    {
        PartColorModel RegisterColor(AddColorDto dto);
        PartColorModel UpdateColor(UpdateColorDto dto);
        IEnumerable<PartColorModel> GetAll();
    }
}
