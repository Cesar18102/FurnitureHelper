using System;
using System.Linq;
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
        private static readonly IConcretePartRepo ConcretePartRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IConcretePartRepo>();
        private static readonly IAccountRepo AccountRepo = DataAccessDependencyHolderWrapper.DataAccessDependencies.Resolve<IAccountRepo>();

        private static readonly PaymentService PaymentService = ServiceDependencyHolder.ServicesDependencies.Resolve<PaymentService>();
        private static readonly SessionService SessionService = ServiceDependencyHolder.ServicesDependencies.Resolve<SessionService>();

        private IDictionary<string, ManufacturerSellModel> PendingOrders = new Dictionary<string, ManufacturerSellModel>();
        private List<int> ReservedConcretePartsIds = new List<int>();

        public PaymentInfo CreateManufacturerTradePromise(AddManufacturerSellDto manufacturerSellDto, string callbackEndpoint)
        {
            if (manufacturerSellDto.NewAccountExtension == null && manufacturerSellDto.ExisitingAccountExtensionId == null)
                throw new NotFoundException("nor old neither new account extension");

            //SessionService.CheckSession(manufacturerSellDto.Session);

            AccountExtensionModel accountExtension = manufacturerSellDto.ExisitingAccountExtensionId == null || 
                                                     !manufacturerSellDto.ExisitingAccountExtensionId.HasValue ?
                                                     Mapper.Map<AccountExtensionDto, AccountExtensionModel>(manufacturerSellDto.NewAccountExtension) :
                                                     AccountRepo.GetExtensionById(manufacturerSellDto.ExisitingAccountExtensionId.Value);

            ManufacturerSellModel sellModel = new ManufacturerSellModel(accountExtension, BuildOrder(manufacturerSellDto.Positions));
            string orderId = Guid.NewGuid().ToString();

            PendingOrders.Add(orderId, sellModel);

            PaymentPrepareModel paymentPrepare = new PaymentPrepareModel(orderId, sellModel.TotalPrice);
            paymentPrepare.CallbackUrl = callbackEndpoint;

            PaymentInfo payment = PaymentService.CreatePaymentToManufacturer(paymentPrepare);
            return payment;
        }

        public ManufacturerSellModel ConfirmManufacturerTradePromise(PaymentConfirmDto paymentConfirmDto)
        {
            PaymentInfo payment = Mapper.Map<PaymentConfirmDto, PaymentInfo>(paymentConfirmDto);

            if(!PaymentService.IsPaymentAuthorized(payment))
                throw new UnauthorizedException();

            string orderId = PaymentService.GetOrderId(payment);

            if (!PendingOrders.ContainsKey(orderId))
                throw new NotFoundException("pending order");

            ManufacturerSellModel sell = PendingOrders[orderId];
            PendingOrders.Remove(orderId);

            if(sell.BuyerAccountExtension.Id == 0)
            {
                //register account extension
            }
            else
            {
                //update account extension last used date
            }

            //add manufacturer sells
            //add ownership

            return sell;
        }

        public IDictionary<ConcretePartModel, float> BuildOrder(IEnumerable<SellPositionDto> sellPositions)
        {
            IDictionary<ConcretePartModel, float> concretePartsOrdered = new Dictionary<ConcretePartModel, float>();
            foreach (SellPositionDto position in sellPositions)
            {
                IEnumerable<ConcretePartModel> concretePartsToOrder = ConcretePartRepo.GetManufacturerPartsForSelling(
                    position.PartId.GetValueOrDefault(),
                    position.Count.GetValueOrDefault(),
                    ReservedConcretePartsIds
                );

                if (concretePartsToOrder.Count() != position.Count)
                    throw new NotFoundException("part " + position.PartId.GetValueOrDefault());

                foreach (ConcretePartModel concretePartToOrder in concretePartsToOrder)
                {
                    float price = concretePartToOrder.Part.Price * concretePartToOrder.SelectedMaterial.PriceCoefficient;
                    concretePartsOrdered.Add(concretePartToOrder, price);
                    ReservedConcretePartsIds.Add(concretePartToOrder.Id);
                }
            }
            return concretePartsOrdered;
        }
    }
}
