using System;
using System.Linq;
using System.Collections.Generic;

using Autofac;

using Models;

using DataAccessHolder;
using DataAccessContract;

using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class BuildService : IBuildService
    {
        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly HashingService HashingService = ServiceDependencyHolder.ServicesDependencies.Resolve<HashingService>();
        private static readonly IFurnitureService FurnitureService = ServiceDependencyHolder.ServicesDependencies.Resolve<IFurnitureService>();

        private static readonly IAccountRepo AccountRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountRepo>();
        private static readonly IFurnitureRepo FurnitureRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IFurnitureRepo>();
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();

        private static readonly IDictionary<string, BuildSessionManager> BuildSessions = new Dictionary<string, BuildSessionManager>();
        private static readonly IDictionary<int, string> UserBuildTokens = new Dictionary<int, string>();
        private static readonly IDictionary<string, string> MacToBuildTokenCache = new Dictionary<string, string>();

        private void CheckBuildSession(BuildSessionDto buildSession)
        {
            SessionService.CheckSession(buildSession.Session);

            if (!BuildSessions.ContainsKey(buildSession.BuildSessionToken))
                throw new NotFoundException("build session");

            BuildSessionManager sessionInfo = BuildSessions[buildSession.BuildSessionToken];

            if (buildSession.Session.UserId.Value != sessionInfo.UserId)
                throw new NotFoundException("build session");
        }

        private BuildSessionManager GetBuildSessionByMac(string mac)
        {
            if (MacToBuildTokenCache.ContainsKey(mac))
                return BuildSessions[MacToBuildTokenCache[mac]];

            AccountModel owner = AccountRepo.GetByOwnedPartMac(mac);

            if (owner == null || !UserBuildTokens.ContainsKey(owner.Id))
                return null;

            string token = UserBuildTokens[owner.Id];
            MacToBuildTokenCache.Add(mac, token);

            return BuildSessions[token];
        }

        public BuildSessionModel InitBuildSession(StartBuildDto startBuildDto)
        {
            SessionService.CheckSession(startBuildDto.Session);
            FurnitureItemModel furniture = FurnitureRepo.Get(startBuildDto.FurnitureId.Value);

            if (furniture == null)
                throw new NotFoundException("furniture");

            if (!FurnitureService.CanBuild(startBuildDto.Session, startBuildDto.FurnitureId.Value))
                throw new NotFoundException("all owned parts");

            int userId = startBuildDto.Session.UserId.Value;

            if (UserBuildTokens.ContainsKey(userId))
                throw new ConflictException("build sessions");

            string token = HashingService.GetHash(Guid.NewGuid().ToString());

            IEnumerable<ConcretePartModel> possiblePartsToUse = ConcretePartRepo
                .GetOwnedByUser(userId)
                .Where(part => !part.IsForSell && !part.IsInUse)
                .ToList();

            BuildSessionManager buildSessionInfo = new BuildSessionManager(
                userId, possiblePartsToUse, furniture, new BuildSessionModel(token)
            );

            UserBuildTokens.Add(userId, token);
            BuildSessions.Add(token, buildSessionInfo);
            return buildSessionInfo.BuildSession;
        }

        public IEnumerable<StepProbeResultModel> GetStepProbeResults(BuildSessionDto buildSession)
        {
            CheckBuildSession(buildSession);
            BuildSessionManager sessionInfo = BuildSessions[buildSession.BuildSessionToken];
            return sessionInfo.StepProbesResults.ToList();
        }

        public TwoPartsConnectionModel GetCurrentStep(BuildSessionDto buildSession)
        {
            CheckBuildSession(buildSession);
            return BuildSessions[buildSession.BuildSessionToken].CurrentStep;
        }

        public IndicatorMapModel HandlePing(ControllerPingDto pingDto)
        {
            BuildSessionManager buildSession = GetBuildSessionByMac(pingDto.Mac);

            if (buildSession == null)
                throw new NotFoundException("build session");

            if (!buildSession.IndicatorMaps.ContainsKey(pingDto.Mac))
                throw new NotFoundException("indicator map");

            return buildSession.IndicatorMaps[pingDto.Mac];
        }

        public StepProbeResultModel HandleStepProbe(StepProbeDto buildActionDto)
        {
            BuildSessionManager buildSession = GetBuildSessionByMac(buildActionDto.Mac);

            if (buildSession == null)
                throw new NotFoundException("build session");

            return buildSession.HandleConnectionProbe(buildActionDto);
        }
    }
}
