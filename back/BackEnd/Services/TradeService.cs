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
        private static readonly IAccountExtensionRepo AccountExtensionRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountExtensionRepo>();
        private static readonly IManufacturerSellsRepo ManufacturerSellRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IManufacturerSellsRepo>();
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();
        private static readonly IOwnershipRepo OwnershipRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IOwnershipRepo>();

        private static readonly PaymentService PaymentService = ServiceDependencyHolder.ServicesDependencies.Resolve<PaymentService>();
        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();

        private IDictionary<string, ManufacturerSellModel> PendingOrders = new Dictionary<string, ManufacturerSellModel>();
        private ICollection<int> ReservedConcretePartsIds = new List<int>();
        private const int SECONDS_TO_EXPIRE_ORDER = 120;

        private IDictionary<string, DateTime> Expires = new Dictionary<string, DateTime>();
        private Task CleanerTask { get; set; }
        private const int CLEANER_DELAY = 60;

        public TradeService()
        {
            CleanerTask = Task.Run(() =>
            {
                while (true)
                {
                    IEnumerable<string> expired = Expires.Where(ex => ex.Value < DateTime.UtcNow)
                                                         .Select(ex => ex.Key).ToList();

                    foreach(string orderId in expired)
                        if (PendingOrders.ContainsKey(orderId))
                            RemoveReservation(orderId, PendingOrders[orderId]);

                    CleanerTask.Wait(CLEANER_DELAY * 1000);
                }
            });
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
            ManufacturerSellModel sellModel = BuildOrderFromManufacturer(orderId, accountExtension, manufacturerSellDto.Positions);

            DateTime expiration = DateTime.UtcNow.AddSeconds(SECONDS_TO_EXPIRE_ORDER);
            PendingOrders.Add(orderId, sellModel);
            Expires.Add(orderId, expiration);

            PaymentPrepareModel paymentPrepare = new PaymentPrepareModel(orderId, sellModel.TotalPrice);
            paymentPrepare.CallbackUrl = callbackEndpoint;
            paymentPrepare.Expired = expiration;

            PaymentInfo payment = PaymentService.CreatePaymentToManufacturer(paymentPrepare);
            return payment;
        }

        public ManufacturerSellModel ConfirmManufacturerTradePromise(PaymentConfirmDto dto)
        {
            return ProtectedExecute<PaymentConfirmDto, ManufacturerSellModel>(paymentConfirmDto =>
            {
                PaymentInfo payment = Mapper.Map<PaymentConfirmDto, PaymentInfo>(paymentConfirmDto);

                if (!PaymentService.IsPaymentAuthorized(payment))
                    throw new UnauthorizedException();

                string orderId = PaymentService.GetOrderId(payment);

                if (!PendingOrders.ContainsKey(orderId))
                    throw new NotFoundException("pending order");

                ManufacturerSellModel sell = PendingOrders[orderId];

                if (!PaymentService.IsSucceed(payment))
                    RemoveReservation(orderId, sell);

                sell.SellDate = DateTime.Now;

                UpdateSellPositionsSellDates(sell);
                RemoveReservation(orderId, sell);
                AttachAccountExtension(sell);

                ManufacturerSellModel createdSell = ManufacturerSellRepo.Create(sell);
                CreateOwnerships(createdSell);

                return createdSell;
            }, dto);
        }

        private void CreateOwnerships(ManufacturerSellModel sell)
        {
            foreach (SellPositionModel sellPosition in sell.SellPositions)
            {
                OwnershipModel ownership = new OwnershipModel(sell.BuyerAccountExtension.AccountId, sellPosition.ConcretePart);
                OwnershipModel createdOwnership = OwnershipRepo.Create(ownership);
            }
        }

        private void AttachAccountExtension(ManufacturerSellModel sell)
        {
            sell.BuyerAccountExtension.LastUsedDate = sell.SellDate;
            AccountExtensionModel accountExtension = sell.BuyerAccountExtension.Id == 0 ?
                                                     AccountExtensionRepo.Create(sell.BuyerAccountExtension) :
                                                     AccountExtensionRepo.Update(sell.BuyerAccountExtension.Id, sell.BuyerAccountExtension);

            Mapper.Map<AccountExtensionModel, AccountExtensionModel>(accountExtension, sell.BuyerAccountExtension);
        }

        private void UpdateSellPositionsSellDates(ManufacturerSellModel sell)
        {
            foreach (SellPositionModel sellPosition in sell.SellPositions)
            {
                sellPosition.ConcretePart.LastSellDate = sell.SellDate;
                ConcretePartRepo.Update(sellPosition.ConcretePart.Id, sellPosition.ConcretePart);
            }
        }

        private void RemoveReservation(string orderId, ManufacturerSellModel sell)
        {
            foreach (SellPositionModel sellPosition in sell.SellPositions)
                ReservedConcretePartsIds.Remove(sellPosition.ConcretePart.Id);

            if (PendingOrders.ContainsKey(orderId))
                PendingOrders.Remove(orderId);

            if (Expires.ContainsKey(orderId))
                Expires.Remove(orderId);
        }

        private ManufacturerSellModel BuildOrderFromManufacturer(string orderId, AccountExtensionModel accountExtension, IEnumerable<SellPositionDto> sellPositions)
        {
            ManufacturerSellModel order = new ManufacturerSellModel(accountExtension);

            foreach (SellPositionDto position in sellPositions)
            {
                IEnumerable<ConcretePartModel> concretePartsToOrder = ConcretePartRepo.GetManufacturerPartsForSelling(
                    position.PartId.GetValueOrDefault(),
                    position.Count.GetValueOrDefault(),
                    ReservedConcretePartsIds
                );

                if (concretePartsToOrder.Count() != position.Count)
                {
                    RemoveReservation(orderId, order);
                    throw new NotFoundException("part " + position.PartId.GetValueOrDefault());
                }

                foreach (ConcretePartModel concretePartToOrder in concretePartsToOrder)
                {
                    float price = concretePartToOrder.Part.Price * concretePartToOrder.SelectedMaterial.PriceCoefficient;
                    SellPositionModel sellPosition = new SellPositionModel(price, concretePartToOrder);

                    ReservedConcretePartsIds.Add(concretePartToOrder.Id);
                    order.SellPositions.Add(sellPosition);
                }
            }

            return order;
        }
    }
}
