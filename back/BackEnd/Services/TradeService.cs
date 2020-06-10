using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using DataAccessHolder;
using DataAccessContract;

using Models;
using Models.Trade;

using Services.Payment;
using ServicesContract;
using ServicesContract.Dto;
using ServicesContract.Exceptions;

namespace Services
{
    public class TradeService : ServiceBase, ITradeService
    {
        private class Sell
        {
            private const int SECONDS_TO_EXPIRE_ORDER = 300;

            public string OrderId { get; private set; }
            public SellModel SellModel { get; private set; }
            public DateTime Expires { get; private set; }
            public PaymentInfo PaymentInfo { get; set; }

            public Sell(string orderId, SellModel sellModel)
            {
                OrderId = orderId;
                SellModel = sellModel;
                Expires = DateTime.UtcNow.AddSeconds(SECONDS_TO_EXPIRE_ORDER);
            }

            public bool Contains(int concretePartId)
            {
                return SellModel.SellPositions.FirstOrDefault(position => position.ConcretePart.Id == concretePartId) != null;
            }
        }

        private static readonly IAccountExtensionRepo AccountExtensionRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountExtensionRepo>();
        private static readonly IManufacturerSellsRepo ManufacturerSellRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IManufacturerSellsRepo>();
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();
        private static readonly IOwnershipRepo OwnershipRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IOwnershipRepo>();

        private static readonly PaymentService PaymentService = ServiceDependencyHolder.ServicesDependencies.Resolve<PaymentService>();
        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();
        private static readonly IPartService PartService = ServiceDependencyHolder.ServicesDependencies.Resolve<IPartService>();

        private IDictionary<string, Sell> PendingOrders = new Dictionary<string, Sell>();

        private Task CleanerTask { get; set; }
        private const int CLEANER_DELAY = 60;

        public TradeService()
        {
            CleanerTask = Task.Run(() =>
            {
                while (true)
                {
                    IEnumerable<string> expired = PendingOrders.Where(order => order.Value.Expires < DateTime.UtcNow)
                                                               .Select(ex => ex.Key).ToList();

                    foreach (string orderId in expired)
                        RemoveReservation(orderId);

                    CleanerTask.Wait(CLEANER_DELAY * 1000);
                }
            });
        }

        public IEnumerable<ConcretePartModel> GetPendingPartsList()
        {
            return PendingOrders.Values.Aggregate(
                new List<ConcretePartModel>(),
                (acc, order) => acc.Concat(order.SellModel.SellPositions.Select(pos => pos.ConcretePart)).ToList()
            );
        }

        public PaymentInfo CreateManufacturerTradePromise(AddManufacturerSellDto manufacturerSellDto, string callbackEndpoint)
        {
            if (manufacturerSellDto.NewAccountExtension == null && manufacturerSellDto.ExisitingAccountExtensionId == null)
                throw new NotFoundException("nor old neither new account extension");

            SessionService.CheckSession(manufacturerSellDto.Session);

            if (manufacturerSellDto.NewAccountExtension != null && manufacturerSellDto.NewAccountExtension.AccountId != manufacturerSellDto.Session.UserId)
                throw new ForbiddenException("account owner");

            AccountExtensionModel accountExtension = manufacturerSellDto.ExisitingAccountExtensionId == null ||
                                                     !manufacturerSellDto.ExisitingAccountExtensionId.HasValue ?
                                                     Mapper.Map<AccountExtensionDto, AccountExtensionModel>(manufacturerSellDto.NewAccountExtension) :
                                                     AccountExtensionRepo.Get(manufacturerSellDto.ExisitingAccountExtensionId.Value);

            if (accountExtension == null)
                throw new NotFoundException("account extension");

            if(accountExtension.AccountId != manufacturerSellDto.Session.UserId)
                throw new ForbiddenException("account owner");

            string orderId = Guid.NewGuid().ToString();
            SellModel sellModel = BuildOrderFromManufacturer(orderId, accountExtension, manufacturerSellDto.Positions);

            Sell sell = new Sell(orderId, sellModel);
            PendingOrders.Add(orderId, sell);

            PaymentPrepareModel paymentPrepare = new PaymentPrepareModel(orderId, sellModel.TotalPrice);
            paymentPrepare.CallbackUrl = callbackEndpoint;
            paymentPrepare.Expired = sell.Expires;

            PaymentInfo payment = PaymentService.CreateFromUserPayment(paymentPrepare);
            sell.PaymentInfo = payment;

            return payment;
        }

        public SellModel ConfirmManufacturerTradePromise(PaymentConfirmDto dto)
        {
            return ProtectedExecute<PaymentConfirmDto, SellModel>(paymentConfirmDto =>
            {
                PaymentInfo payment = Mapper.Map<PaymentConfirmDto, PaymentInfo>(paymentConfirmDto);
                string orderId = PaymentService.GetOrderId(payment);

                if (!PendingOrders.ContainsKey(orderId))
                    throw new NotFoundException("pending order");

                bool autorizedPayment = PaymentService.IsPaymentAuthorized(payment, PendingOrders[orderId].PaymentInfo);
                if (!autorizedPayment)
                    throw new UnauthorizedException();

                Sell sell = PendingOrders[orderId];

                if (!PaymentService.IsSucceed(payment))
                {
                    RemoveReservation(orderId);
                    throw new NotFoundException("order");
                }

                sell.SellModel.SellDate = DateTime.Now;

                UpdateSellPositionsSellDates(sell.SellModel);
                AttachAccountExtension(sell.SellModel);

                SellModel createdSell = ManufacturerSellRepo.Create(sell.SellModel);
                CreateOwnerships(createdSell);

                RemoveReservation(orderId);

                return createdSell;
            }, dto);
        }

        private void CreateOwnerships(SellModel sell)
        {
            foreach (SellPositionModel sellPosition in sell.SellPositions)
            {
                OwnershipModel ownership = new OwnershipModel(sell.BuyerAccountExtension.AccountId, sellPosition.ConcretePart);
                OwnershipModel createdOwnership = OwnershipRepo.Create(ownership);
            }
        }

        private void AttachAccountExtension(SellModel sell)
        {
            sell.BuyerAccountExtension.LastUsedDate = sell.SellDate;
            AccountExtensionModel accountExtension = sell.BuyerAccountExtension.Id == 0 ?
                                                     AccountExtensionRepo.Create(sell.BuyerAccountExtension) :
                                                     AccountExtensionRepo.Update(sell.BuyerAccountExtension.Id, sell.BuyerAccountExtension);

            Mapper.Map<AccountExtensionModel, AccountExtensionModel>(accountExtension, sell.BuyerAccountExtension);
        }

        private void UpdateSellPositionsSellDates(SellModel sell)
        {
            foreach (SellPositionModel sellPosition in sell.SellPositions)
            {
                sellPosition.ConcretePart.LastSellDate = sell.SellDate;
                ConcretePartRepo.Update(sellPosition.ConcretePart.Id, sellPosition.ConcretePart);
            }
        }

        private void RemoveReservation(string orderId)
        {
            if (PendingOrders.ContainsKey(orderId))
                PendingOrders.Remove(orderId);
        }

        private bool IsReserved(int concretePartId)
        {
            return PendingOrders.Values.FirstOrDefault(order => order.Contains(concretePartId)) != null;
        }

        private IEnumerable<ConcretePartModel> GetOrderFromSource(
            IEnumerable<SellPositionDto> needed, 
            IEnumerable<ConcretePartModel> source, 
            string notEnoughMessage
        )
        {
            ICollection<ConcretePartModel> result = new List<ConcretePartModel>();

            foreach (SellPositionDto sellPosition in needed)
            {
                int partId = sellPosition.PartId.GetValueOrDefault();
                int materialId = sellPosition.MaterialId.GetValueOrDefault();
                int colorId = sellPosition.ColorId.GetValueOrDefault();
                int count = sellPosition.Count.GetValueOrDefault();

                IEnumerable<ConcretePartModel> possible = source.Where(part => 
                    part.Part.Id == partId &&
                    part.SelectedMaterial.Id == materialId &&
                    part.SelectedColor.Id == colorId
                );

                IEnumerable<ConcretePartModel> order = possible.Where(part => !IsReserved(part.Id)).Take(count).ToList();

                if (order.Count() != count)
                    throw new NotFoundException($"{notEnoughMessage} {partId}");

                result = result.Concat(order).ToList();
            }

            return result;
        }

        private SellModel BuildOrderFromManufacturer(string orderId, AccountExtensionModel accountExtension, IEnumerable<SellPositionDto> sellPositions)
        {
            SellModel sell = new SellModel(accountExtension);

            IEnumerable<ConcretePartModel> source = ConcretePartRepo.GetStored();
            IEnumerable<ConcretePartModel> order = GetOrderFromSource(sellPositions, source, "part ");

            foreach (ConcretePartModel concretePart in order)
            {
                float price = concretePart.Part.Price.Value * concretePart.SelectedMaterial.PriceCoefficient.Value;
                SellPositionModel sellPosition = new SellPositionModel(price, concretePart);
                sell.SellPositions.Add(sellPosition);
            }

            return sell;
        }

        public IEnumerable<ConcretePartModel> BidParts(TradeOwnedPartsDto partsToBid)
        {
            SessionService.CheckSession(partsToBid.Session);

            IEnumerable<ConcretePartModel> source = PartService.GetOwnedConcrete(partsToBid.Session)
                .Where(part => !part.IsInUse && !part.IsForSell).ToList();

            IEnumerable<ConcretePartModel> forSell = GetOrderFromSource(partsToBid.Positions, source, "owned part");
            return ConcretePartRepo.MarkPartsForSell(forSell);
        }

        public IEnumerable<ConcretePartModel> UnbidParts(TradeOwnedPartsDto partsToUnbid)
        {
            SessionService.CheckSession(partsToUnbid.Session);

            IEnumerable<ConcretePartModel> source = PartService.GetOwnedConcrete(partsToUnbid.Session)
                .Where(part => !part.IsInUse && part.IsForSell).ToList();

            IEnumerable<ConcretePartModel> forSell = GetOrderFromSource(partsToUnbid.Positions, source, "bid part");
            return ConcretePartRepo.UnmarkPartsForSell(forSell);
        }

        /*public PaymentInfo CreateUserToUserTradePromise(TradeOwnedPartsDto partsToBuy)
        {
            SessionService.CheckSession(partsToBuy.Session);

            IEnumerable<int> owned = ConcretePartRepo
                .GetOwnedByUser(partsToBuy.Session.UserId.GetValueOrDefault())
                .Select(part => part.Id).ToList();

            IEnumerable<ConcretePartModel> source = ConcretePartRepo.GetForSellParts().Where(part => !owned.Contains(part.Id));
            IEnumerable<ConcretePartModel> order = GetOrderFromSource(partsToBuy.Positions, source, "bid part");

            //Sell

            PaymentService.CreateFromUserPayment()
        }*/
    }
}
