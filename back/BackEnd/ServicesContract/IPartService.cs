using System.Collections.Generic;

using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IPartService
    {
        PartModel RegisterPart(AddPartDto dto);
        PartModel UpdatePart(UpdatePartDto dto);
        PartModel DeletePart(DeleteDto dto);

        ConcretePartModel RegisterConcretePart(AddConcretePartDto dto);

        PartStore GetStore();
        PartModel Get(int partId);
        ControllerConfigModel GetControllerConfig(ControllerPingDto pingDto);

        PartStore GetUserBids();
        PartStore GetOwned(SessionDto ownerSession);
        InvariantPartStore GetOwnedInvariant(SessionDto ownerSession);
        IEnumerable<ConcretePartModel> GetOwnedConcrete(SessionDto session);
    }
}
