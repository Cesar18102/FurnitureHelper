using System.Collections.Generic;

using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IBuildService
    {
        BuildSessionModel InitBuildSession(StartBuildDto startBuildDto);
        BuildSessionModel GetBuildSession(SessionDto session);

        TwoPartsConnectionModel GetCurrentStep(BuildSessionDto buildSession);
        GlobalPartsConnectionModel GetCurrentGlobalStep(BuildSessionDto buildSession);
        IEnumerable<StepProbeResultModel> GetStepProbeResults(BuildSessionDto buildSession);

        IndicatorMapModel HandlePing(ControllerPingDto pingDto);
        StepProbeResultModel HandleStepProbe(StepProbeDto buildActionDto);
    }
}
