using System.Threading.Tasks;

using Autofac;

using ServerAccess;
using Services.Declaration;
using Models.Dto.PartStore;

namespace Services.Implementation
{
    public class PartService : IPartService
    {
        private const string GET_PARTS_ENDPOINT = "part/get";

        private static readonly IServer Server = ServerHolder.Dependencies.Resolve<IServer>();

        public async Task<PartStoreDto> GetParts()
        {
            return await Server.SendGet<PartStoreDto>(
                ServerHolder.SERVER_URL + GET_PARTS_ENDPOINT
            );
        }
    }
}
