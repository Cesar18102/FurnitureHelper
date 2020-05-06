using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IPartService
    {
        PartModel RegisterPart(AddPartDto dto);
        PartModel UpdatePart(UpdatePartDto dto);
        ConcretePartModel RegisterConcretePart(AddConcretePartDto dto);
    }
}
