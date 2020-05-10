using System.Collections.Generic;

using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IPartService
    {
        PartModel RegisterPart(AddPartDto dto);
        PartModel UpdatePart(UpdatePartDto dto);
        ConcretePartModel RegisterConcretePart(AddConcretePartDto dto);

        PartStore GetStore();
        PartStore GetOwned(SessionDto ownerSession);
        IEnumerable<ConcretePartModel> GetOwnedConcrete(SessionDto session);
    }
}
