using System.Collections.Generic;

using Models;
using Models.Trade;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface ITradeService
    {
        PaymentInfo CreateManufacturerTradePromise(AddManufacturerSellDto manufacturerSellDto, string callbackEndpoint);
        SellModel ConfirmManufacturerTradePromise(PaymentConfirmDto paymentConfirmDto);

        IEnumerable<ConcretePartModel> GetPendingPartsList();

        //PaymentInfo CreateUserToUserTradePromise(TradeOwnedPartsDto partsToBuy);
        //SellModel<UserToUserSellPosition> ConfirmUserToUserTradePromise(PaymentConfirmDto paymentConfirmDto);

        IEnumerable<ConcretePartModel> BidParts(TradeOwnedPartsDto partsToBid);
        IEnumerable<ConcretePartModel> UnbidParts(TradeOwnedPartsDto partsToUnbid);
    }
}
