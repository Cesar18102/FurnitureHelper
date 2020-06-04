using System.Collections.Generic;

using Models;

using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IColorService
    {
        PartColorModel RegisterColor(AddColorDto dto);
        PartColorModel UpdateColor(UpdateColorDto dto);
        PartColorModel DeleteColor(DeleteDto dto);
        
        PartColorModel Get(int colorId);
        IEnumerable<PartColorModel> GetAll();
    }
}
