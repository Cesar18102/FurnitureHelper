using Models;
using Models.Trade;
using ServicesContract.Dto;

namespace ServicesContract
{
    public interface ITradeService
    {
        PaymentInfo CreateManufacturerTradePromise(AddManufacturerSellDto manufacturerSellDto, string callbackEndpoint);
        ManufacturerSellModel ConfirmManufacturerTradePromise(PaymentConfirmDto paymentConfirmDto);
    }
}
