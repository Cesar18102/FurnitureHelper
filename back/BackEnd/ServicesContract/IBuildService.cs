using System.Collections.Generic;

using Models;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface IBuildService
    {
        BuildSessionModel InitBuildSession(StartBuildDto startBuildDto);

        TwoPartsConnectionModel GetCurrentStep(BuildSessionDto buildSession);
        IEnumerable<StepProbeResultModel> PopStepProbeResults(BuildSessionDto buildSession);

        IndicatorMapModel HandlePing(ControllerPingDto pingDto);
        StepProbeResultModel HandleStepProbe(StepProbeDto buildActionDto);
    }
}
