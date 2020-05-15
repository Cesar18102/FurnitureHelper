using System.Collections.Generic;

using Models;

using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IMaterialService
    {
        MaterialModel RegisterMaterial(AddMaterialDto dto);
        MaterialModel UpdateMaterial(UpdateMaterialDto dto);

        MaterialModel Get(int materialId);
        IEnumerable<MaterialModel> GetAll();
    }
}
