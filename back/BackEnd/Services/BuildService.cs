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

        private static readonly IFurnitureRepo FurnitureRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IFurnitureRepo>();
        private static readonly IFurnitureService FurnitureService = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IFurnitureService>();
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();

        private static readonly IDictionary<string, BuildSessionInfo> BuildSessions = new Dictionary<string, BuildSessionInfo>();
        private static readonly IDictionary<string, string> ContollersBuildTokens = new Dictionary<string, string>();

        private void CheckBuildSession(BuildSessionDto buildSession)
        {
            SessionService.CheckSession(buildSession.Session);

            if(!BuildSessions.ContainsKey(buildSession.BuildSessionToken))
                throw new NotFoundException("build session");

            BuildSessionInfo sessionInfo = BuildSessions[buildSession.BuildSessionToken];

            if (buildSession.Session.UserId.Value != sessionInfo.UserId)
                throw new NotFoundException("build session");
        }

        public BuildSessionModel InitBuildSession(StartBuildDto startBuildDto)
        {
            SessionService.CheckSession(startBuildDto.Session);
            FurnitureItemModel furniture = FurnitureRepo.Get(startBuildDto.FurnitureId.Value);

            if (furniture == null)
                throw new NotFoundException("furniture");

            if (!FurnitureService.CanBuild(startBuildDto.Session, startBuildDto.FurnitureId.Value))
                throw new NotFoundException("all owned parts");

            string token = HashingService.GetHash(Guid.NewGuid().ToString());

            int userId = startBuildDto.Session.UserId.Value;
            IEnumerable<ConcretePartModel> possiblePartsToUse = ConcretePartRepo
                .GetOwnedByUser(userId)
                .Where(part => !part.IsForSell && !part.IsInUse)
                .ToList();

            BuildSessionInfo buildSessionInfo = new BuildSessionInfo(
                userId, possiblePartsToUse, furniture, new BuildSessionModel(token)
            );

            BuildSessions.Add(token, buildSessionInfo);
            return buildSessionInfo.BuildSession;
        }

        public IEnumerable<StepProbeResultModel> PopStepProbeResults(BuildSessionDto buildSession)
        {
            CheckBuildSession(buildSession);
            BuildSessionInfo sessionInfo = BuildSessions[buildSession.BuildSessionToken];

            IEnumerable<StepProbeResultModel> stepProbes =  sessionInfo.StepProbes.ToList();
            sessionInfo.StepProbes.Clear();
            return stepProbes;
        }

        public TwoPartsConnectionModel GetCurrentStep(BuildSessionDto buildSession)
        {
            CheckBuildSession(buildSession);
            return BuildSessions[buildSession.BuildSessionToken].CurrentStep;
        }

        public IndicatorMapModel HandlePing(ControllerPingDto pingDto)
        {
            pingDto.Mac = pingDto.Mac.ToUpper();
            if (ContollersBuildTokens.ContainsKey(pingDto.Mac))
            {
                string token = ContollersBuildTokens[pingDto.Mac];
                BuildSessionInfo buildSession = BuildSessions[token];

                if (!buildSession.IndicatorMaps.ContainsKey(pingDto.Mac))
                    throw new NotFoundException("indicator map");

                return buildSession.IndicatorMaps[pingDto.Mac];
            }

            return new IndicatorMapModel();
        }

        public StepProbeResultModel HandleStepProbe(StepProbeDto buildActionDto)
        {
            buildActionDto.Mac = buildActionDto.Mac.ToUpper();
            if (ContollersBuildTokens.ContainsKey(buildActionDto.Mac))
            {
                string token = ContollersBuildTokens[buildActionDto.Mac];
                BuildSessionInfo buildSession = BuildSessions[token];
                IEnumerable<int> wrongPins = buildSession.CheckConnection(buildActionDto.Mac, buildActionDto.ActiveReaders);
                return new StepProbeResultModel(wrongPins);
            }

            throw new NotFoundException("part in build");
        }
    }
}
