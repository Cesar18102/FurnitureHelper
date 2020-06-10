using System;
using System.Linq;
using System.Collections.Generic;

using ServicesContract.Dto;
using System.Diagnostics;

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

        public GlobalPartsConnectionModel CurrentGlobalStep => Furniture.GlobalConnections.ElementAt(GlobalOrderNumber);

        private IEnumerable<ConcretePartModel> OwnedParts;
        public ICollection<StepProbeResultModel> StepProbesResults { get; private set; } = new List<StepProbeResultModel>();
        public IDictionary<string, IndicatorMapModel> IndicatorMaps { get; private set; } = new Dictionary<string, IndicatorMapModel>();
        public IDictionary<string, ICollection<int>> UsedReaders { get; private set; } = new Dictionary<string, ICollection<int>>();

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
                if (CurrentStepPinsConnected.mac != null)
                {
                    if (!UsedReaders.ContainsKey(CurrentStepPinsConnected.mac))
                        UsedReaders.Add(CurrentStepPinsConnected.mac, new List<int>());

                    foreach (int pin in CurrentStepPinsConnected.pins)
                        UsedReaders[CurrentStepPinsConnected.mac].Add(pin);

                    if (!UsedParts.ContainsKey(CurrentStep.UsedPartId) && CurrentStepUsedParts.ContainsKey(CurrentStepPinsConnected.mac))
                        UsedParts.Add(CurrentStep.UsedPartId, CurrentStepUsedParts[CurrentStepPinsConnected.mac]);

                    CurrentStepPinsConnected = (null, new List<int>());
                }

                if (CurrentStepPinsOtherConnected.mac != null)
                {
                    if (!UsedReaders.ContainsKey(CurrentStepPinsOtherConnected.mac))
                        UsedReaders.Add(CurrentStepPinsOtherConnected.mac, new List<int>());

                    foreach (int pin in CurrentStepPinsOtherConnected.pins)
                        UsedReaders[CurrentStepPinsOtherConnected.mac].Add(pin);

                    if (!UsedParts.ContainsKey(CurrentStep.UsedPartOtherId) && CurrentStepUsedPartsOther.ContainsKey(CurrentStepPinsOtherConnected.mac))
                        UsedParts.Add(CurrentStep.UsedPartOtherId, CurrentStepUsedPartsOther[CurrentStepPinsOtherConnected.mac]);

                    CurrentStepPinsOtherConnected = (null, new List<int>());
                }

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
                OwnedParts.Where(part => part.Part.Id == CurrentStep.Part.Id && !UsedPartIds.Contains(part.Id)).ToDictionary(part => part.ControllerMac, part => part);

            CurrentStepUsedPartsOther = UsedParts.ContainsKey(CurrentStep.UsedPartOtherId) ?
                new Dictionary<string, ConcretePartModel>() { { UsedParts[CurrentStep.UsedPartOtherId].ControllerMac, UsedParts[CurrentStep.UsedPartOtherId] } } :
                OwnedParts.Where(part => part.Part.Id == CurrentStep.PartOther.Id && !UsedPartIds.Contains(part.Id)).ToDictionary(part => part.ControllerMac, part => part);
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
                if (CurrentStepPinsConnected.pins.Count == 0)
                    CurrentStepPinsConnected.mac = null;

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
                if (CurrentStepPinsOtherConnected.pins.Count == 0)
                    CurrentStepPinsOtherConnected.mac = null;

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
            /*if (TryDetachPin(mac, pin, CurrentStepPinsConnected) || TryDetachPin(mac, pin, CurrentStepPinsOtherConnected))
                return;*/

            if (TryDetachPin(mac, pin, CurrentStepPinsConnected))
            {
                if (CurrentStepPinsConnected.pins.Count == 0)
                    CurrentStepPinsConnected.mac = null;

                return;
            }


            if(TryDetachPin(mac, pin, CurrentStepPinsOtherConnected))
            {
                if (CurrentStepPinsOtherConnected.pins.Count == 0)
                    CurrentStepPinsOtherConnected.mac = null;

                return;
            }

            throw new NotAttachedException();
        }

        private bool IsStepDone()
        {
            List<int> pins = new List<int>() { CurrentStep.ConnectionHelper.ReaderPinNumber, CurrentStep.ConnectionHelper.ReaderPinNumberOther };
            List<int> pinsOther = new List<int>() { CurrentStep.ConnectionHelperOther.ReaderPinNumber, CurrentStep.ConnectionHelperOther.ReaderPinNumberOther };

            return pins.Intersect(CurrentStepPinsConnected.pins).Count() == 2 && pinsOther.Intersect(CurrentStepPinsOtherConnected.pins).Count() == 2;
        }

        private StepProbeResultModel CheckConnectionProbe(StepProbeDto stepProbe)
        {
            string probeId = Guid.NewGuid().ToString();
            foreach (PinStateChange pinState in stepProbe.PinStateChanges)
            {
                if (pinState.Change == StateChange.UNCHANGED)
                    continue;

                Debug.WriteLine($"FROM {stepProbe.Mac} state={pinState.Change.ToString()} ON {pinState.PinNumber}");

                if (IsCurrentStepReaderPin(pinState.PinNumber))
                {
                    if (pinState.Change == StateChange.ATTACHED)
                    {
                        try { lock (Mutex) { AttachPin(stepProbe.Mac, pinState.PinNumber); } }
                        catch (MisAttachException ex) { return new StepProbeResultModel(probeId, ProbeStatus.ERROR); }
                        catch (AlreadyAttachedException ex) { return new StepProbeResultModel(probeId, ProbeStatus.PENDING); }
                    }
                    else if (pinState.Change == StateChange.DETACHED)
                    {
                        try { lock (Mutex) { DetachPin(stepProbe.Mac, pinState.PinNumber); } }
                        catch(NotAttachedException ex) { return new StepProbeResultModel(probeId, ProbeStatus.PENDING); }
                    }
                }
                else
                    return new StepProbeResultModel(probeId, ProbeStatus.ERROR);
            }

            if (IsStepDone())
            {
                lock (Mutex) { NextStep(); }

                return new StepProbeResultModel(probeId, Finished ? ProbeStatus.FINISHED : ProbeStatus.DONE);
            }
            else
                return new StepProbeResultModel(probeId, ProbeStatus.PENDING);
        }

        public StepProbeResultModel HandleConnectionProbe(StepProbeDto stepProbe)
        {
            StepProbeResultModel result = CheckConnectionProbe(stepProbe);

            if (result.Status != ProbeStatus.PENDING)
                StepProbesResults.Add(result);

            return result;
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
