using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using Models;
using ServerAccess;
using Services.Declaration;

namespace Services.Implementation
{
    internal class FurnitureService : IFurnitureService
    {
        private const string GET_FURNITURE_ENDPOINT = "Furniture/Get";

        private static readonly IServer Server = ServerHolder.Dependencies.Resolve<IServer>();

        public async Task<IEnumerable<FurnitureItemDto>> GetFurnitureItems()
        {
            return await Server.SendGet<IEnumerable<FurnitureItemDto>>(
                ServerHolder.SERVER_URL + GET_FURNITURE_ENDPOINT
            );
        }
    }
}
