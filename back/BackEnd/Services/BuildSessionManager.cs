using System;
using System.Linq;
using System.Collections.Generic;

using ServicesContract.Dto;

namespace Models
{
    public class BuildSessionManager
    {
        private class MisAttachException : Exception { }
        private class AlreadyAttachedException : Exception { }
        private class NotAttachedException : Exception { }

        public int UserId { get; private set; }
        public FurnitureItemModel Furniture { get; private set; }
        public BuildSessionModel BuildSession { get; private set; }

        public TwoPartsConnectionModel CurrentStep => Furniture.GlobalConnections.ElementAt(GlobalOrderNumber)
                                                               .SubConnections.ElementAt(SubOrderNumber);

        private IEnumerable<ConcretePartModel> OwnedParts;
        public ICollection<StepProbeResultModel> StepProbesResults { get; private set; } = new List<StepProbeResultModel>();
        public IDictionary<string, IndicatorMapModel> IndicatorMaps { get; private set; } = new Dictionary<string, IndicatorMapModel>();

        private int SubOrderNumber = -1;
        private int GlobalOrderNumber = 0;
        private bool Finished = false;
        private object Mutex = new object();

        public IEnumerable<int> UsedPartIds => UsedParts.Values.Select(part => part.Id);
        private IDictionary<int, ConcretePartModel> UsedParts = new Dictionary<int, ConcretePartModel>();

        private IDictionary<string, ConcretePartModel> CurrentStepUsedParts = new Dictionary<string, ConcretePartModel>();
        private IDictionary<string, ConcretePartModel> CurrentStepUsedPartsOther = new Dictionary<string, ConcretePartModel>();

        private (string mac, ICollection<int> pins) CurrentStepPinsConnected = (null, new List<int>());
        private (string mac, ICollection<int> pins) CurrentStepPinsOtherConnected = (null, new List<int>());

        public void NextStep()
        {
            if (!Finished)
            {
                int count = Furniture.GlobalConnections.ElementAt(GlobalOrderNumber).SubConnections.Count;

                ++SubOrderNumber;
                GlobalOrderNumber += SubOrderNumber / count;
                SubOrderNumber %= count;

                if (GlobalOrderNumber >= Furniture.GlobalConnections.Count)
                {
                    Finished = true;
                    return;
                }

                UpdateStepParts();
                UpdateIndicators();
            }
        }

        private void UpdateStepParts()
        {
            CurrentStepUsedParts = UsedParts.ContainsKey(CurrentStep.UsedPartId) ?
                new Dictionary<string, ConcretePartModel>() { { UsedParts[CurrentStep.UsedPartId].ControllerMac, UsedParts[CurrentStep.UsedPartId] } } :
                OwnedParts.Where(part => part.Part.Id == CurrentStep.Part.Id).ToDictionary(part => part.ControllerMac, part => part);

            CurrentStepUsedPartsOther = UsedParts.ContainsKey(CurrentStep.UsedPartOtherId) ?
                new Dictionary<string, ConcretePartModel>() { { UsedParts[CurrentStep.UsedPartOtherId].ControllerMac, UsedParts[CurrentStep.UsedPartOtherId] } } :
                OwnedParts.Where(part => part.Part.Id == CurrentStep.PartOther.Id).ToDictionary(part => part.ControllerMac, part => part);
        }

        private void UpdateIndicators()
        {
            IndicatorMaps.Clear();

            IndicatorMapModel indicatorMap = new IndicatorMapModel();
            indicatorMap.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelper.IndicatorPinNumber);

            foreach (ConcretePartModel part in CurrentStepUsedParts.Values)
                IndicatorMaps.Add(part.ControllerMac, indicatorMap);

            if (CurrentStep.Part.Id == CurrentStep.PartOther.Id)
                indicatorMap.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelperOther.IndicatorPinNumber);
            else
            {
                IndicatorMapModel indicatorMapOther = new IndicatorMapModel();
                indicatorMapOther.EnabledIndicatorPins.Add(CurrentStep.ConnectionHelperOther.IndicatorPinNumber);

                foreach (ConcretePartModel part in CurrentStepUsedPartsOther.Values)
                    IndicatorMaps.Add(part.ControllerMac, indicatorMapOther);
            }
        }

        private bool IsCurrentStepPartReaderPin(int pin)
        {
            return CurrentStep.ConnectionHelper.ReaderPinNumber == pin ||
                   CurrentStep.ConnectionHelper.ReaderPinNumberOther == pin;
        }

        private bool IsCurrentStepPartOtherReaderPin(int pin)
        {
            return CurrentStep.ConnectionHelperOther.ReaderPinNumber == pin ||
                   CurrentStep.ConnectionHelperOther.ReaderPinNumberOther == pin;
        }

        private bool IsCurrentStepReaderPin(int pin)
        {
            return IsCurrentStepPartReaderPin(pin) || IsCurrentStepPartOtherReaderPin(pin);
        }

        private void AttachPin(string mac, int pin)
        {
            if(CurrentStepUsedParts.ContainsKey(mac) && IsCurrentStepPartReaderPin(pin) && CurrentStepPinsOtherConnected.mac != mac)
            {
                if (CurrentStepPinsConnected.mac == mac)
                {
                    if (CurrentStepPinsConnected.pins.Contains(pin))
                        throw new AlreadyAttachedException();

                    CurrentStepPinsConnected.pins.Add(pin);
                }
                else if (CurrentStepPinsConnected.mac == null)
                {
                    if (!IsCurrentStepPartReaderPin(pin))
                        throw new MisAttachException();

                    CurrentStepPinsConnected.mac = mac;
                    CurrentStepPinsConnected.pins.Add(pin);
                }
            }
            else if(CurrentStepUsedPartsOther.ContainsKey(mac) && IsCurrentStepPartOtherReaderPin(pin) && CurrentStepPinsConnected.mac != mac)
            {
                if (CurrentStepPinsOtherConnected.mac == mac)
                {
                    if (CurrentStepPinsOtherConnected.pins.Contains(pin))
                        throw new AlreadyAttachedException();

                    CurrentStepPinsOtherConnected.pins.Add(pin);
                }
                else if (CurrentStepPinsOtherConnected.mac == null)
                {
                    if (!IsCurrentStepPartOtherReaderPin(pin))
                        throw new MisAttachException();

                    CurrentStepPinsOtherConnected.mac = mac;
                    CurrentStepPinsOtherConnected.pins.Add(pin);
                }
                else
                    throw new MisAttachException();
            }
            else
                throw new MisAttachException();
        }

        private bool TryDetachPin(string mac, int pin, (string mac, ICollection<int> pins) holder)
        {
            if (holder.mac == mac)
            {
                if (!holder.pins.Contains(pin))
                    return false;

                holder.pins.Remove(pin);
                return true;
            }

            return false;
        }

        private void DetachPin(string mac, int pin)
        {
            if (TryDetachPin(mac, pin, CurrentStepPinsConnected) || TryDetachPin(mac, pin, CurrentStepPinsOtherConnected))
                return;

            throw new NotAttachedException();
        }

        private bool IsStepDone()
        {
            List<int> pins = new List<int>() { CurrentStep.ConnectionHelper.ReaderPinNumber, CurrentStep.ConnectionHelper.ReaderPinNumberOther };
            List<int> pinsOther = new List<int>() { CurrentStep.ConnectionHelperOther.ReaderPinNumber, CurrentStep.ConnectionHelperOther.ReaderPinNumberOther };

            return pins.Intersect(CurrentStepPinsConnected.pins).Count() == 2 && pinsOther.Intersect(CurrentStepPinsOtherConnected.pins).Count() == 2;
        }

        public StepProbeResultModel HandleConnectionProbe(StepProbeDto stepProbe)
        {
            foreach (PinStateChange pinState in stepProbe.PinStateChanges)
            {
                if (IsCurrentStepReaderPin(pinState.PinNumber))
                {
                    if (pinState.Change == StateChange.ATTACHED)
                    {
                        try { lock (Mutex) { AttachPin(stepProbe.Mac, pinState.PinNumber); } }
                        catch (MisAttachException ex) { return new StepProbeResultModel(ProbeStatus.ERROR); }
                        catch (AlreadyAttachedException ex) { return new StepProbeResultModel(ProbeStatus.ERROR); }
                    }
                    else if (pinState.Change == StateChange.DETACHED)
                    {
                        try { lock (Mutex) { DetachPin(stepProbe.Mac, pinState.PinNumber); } }
                        catch(NotAttachedException ex) { return new StepProbeResultModel(ProbeStatus.ERROR); }
                    }
                }
                else
                    return new StepProbeResultModel(ProbeStatus.ERROR);
            }

            if (IsStepDone())
            {
                lock (Mutex) { NextStep(); }
                return new StepProbeResultModel(Finished ? ProbeStatus.FINISHED : ProbeStatus.DONE);
            }
            else
                return new StepProbeResultModel(ProbeStatus.PENDING);
        }

        public BuildSessionManager(int userId, IEnumerable<ConcretePartModel> possibleParts, FurnitureItemModel furniture, BuildSessionModel buildSession)
        {
            UserId = userId;
            BuildSession = buildSession;

            Furniture = furniture;
            Furniture.SortConnections();
            OwnedParts = possibleParts;

            NextStep();
        }
    }
}
