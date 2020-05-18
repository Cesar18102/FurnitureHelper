using System;
using System.Linq;
using System.Collections.Generic;

namespace Models
{
    public class BuildSessionInfo : IModel
    {
        public event EventHandler<EventArgs> Finished;

        public int UserId { get; private set; }
        public FurnitureItemModel Furniture { get; private set; }
        public BuildSessionModel BuildSession { get; private set; }

        public TwoPartsConnectionModel CurrentStep { get; private set; }
        public ICollection<StepProbeResultModel> StepProbes { get; private set; } = new List<StepProbeResultModel>();
        public IDictionary<string, IndicatorMapModel> IndicatorMaps { get; private set; } = new Dictionary<string, IndicatorMapModel>();

        private int SubOrderNumber = -1;
        private int GlobalOrderNumber = -1;
        private IEnumerable<ConcretePartModel> PossibleParts;

        private ConcretePartModel CurrentStepPartConnected;
        private IEnumerable<int> CurrentStepPartConnectedActivePins;

        private IEnumerable<ConcretePartModel> CurrentStepUsedParts;
        private IEnumerable<ConcretePartModel> CurrentStepUsedPartsOther;

        public void NextStep()
        {
            if (GlobalOrderNumber >= Furniture.GlobalConnections.Count)
                return;

            if (SubOrderNumber + 1 >= Furniture.GlobalConnections.ElementAt(GlobalOrderNumber).SubConnections.Count)
            {
                ++GlobalOrderNumber;
                SubOrderNumber = 0;
            }

            if (GlobalOrderNumber >= Furniture.GlobalConnections.Count)
            {
                Finished?.Invoke(this, new EventArgs());
                return;
            }

            CurrentStep = Furniture.GlobalConnections.ElementAt(GlobalOrderNumber).SubConnections.ElementAt(++SubOrderNumber);
            UpdateIndicators();
        }

        public void UpdateIndicators()
        {
            IndicatorMaps.Clear();

            CurrentStepUsedParts = PossibleParts.Where(part => part.Id == CurrentStep.Part.Id);

            IndicatorMapModel indicatorMap = new IndicatorMapModel();
            indicatorMap.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelper.IndicatorPinNumber);

            bool sameParts = CurrentStep.Part.Id == CurrentStep.PartOther.Id;

            if (sameParts)
                indicatorMap.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelperOther.IndicatorPinNumber);

            foreach (ConcretePartModel part in CurrentStepUsedParts)
                IndicatorMaps.Add(part.ControllerMac, indicatorMap);

            if (sameParts)
            {
                CurrentStepUsedPartsOther = CurrentStepUsedParts;
                return;
            }

            CurrentStepUsedPartsOther = PossibleParts.Where(part => part.Id == CurrentStep.PartOther.Id);

            IndicatorMapModel indicatorMapOther = new IndicatorMapModel();
            indicatorMapOther.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelperOther.IndicatorPinNumber);

            foreach (ConcretePartModel part in CurrentStepUsedPartsOther)
                IndicatorMaps.Add(part.ControllerMac, indicatorMapOther);
        }

        public IEnumerable<int> CheckConnection(string mac, IEnumerable<int> activePins)
        {
            //TODO
            ConcretePartModel concretePart = CurrentStepUsedParts.FirstOrDefault(part => part.ControllerMac == mac);

            if(concretePart == null)
                concretePart = CurrentStepUsedPartsOther.FirstOrDefault(part => part.ControllerMac == mac);

            if(CurrentStepPartConnected == null || CurrentStepPartConnected.ControllerMac == mac)
            {
                CurrentStepPartConnectedActivePins = activePins;
                CurrentStepPartConnected = concretePart;
                return new List<int>();
            }

            if(CurrentStepPartConnected.Id == CurrentStep.Part.Id && concretePart.Id == CurrentStep.PartOther.Id)
            {
                return null;
                //if(CurrentStepPartConnectedActivePins.Contains())
            }
            else if(CurrentStepPartConnected.Id == CurrentStep.PartOther.Id && concretePart.Id == CurrentStep.Part.Id)
            {
                return null;
            }
            else
                return activePins;
        }

        public BuildSessionInfo(int userId, IEnumerable<ConcretePartModel> possibleParts, FurnitureItemModel furniture, BuildSessionModel buildSession)
        {
            UserId = userId;
            BuildSession = buildSession;

            Furniture = furniture;
            Furniture.SortConnections();
            PossibleParts = possibleParts;

            CurrentStep = furniture.GlobalConnections.ElementAt(++GlobalOrderNumber).SubConnections.ElementAt(++SubOrderNumber);
            UpdateIndicators();
        }
    }
}
