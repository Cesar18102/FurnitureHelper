using System.Net.Http;
using System.Web.Http;

using Autofac;

using Models;
using Models.Trade;
using ServiceHolder;
using ServicesContract;
using ServicesContract.Dto;

namespace BackEnd.Controllers
{
    public class TradeController : ApiController
    {
        private ITradeService TradeService = ServiceDependencyHolderWrapper.ServicesDependencies.Resolve<ITradeService>();

        [HttpPost]
        public HttpResponseMessage BuyFromManufacturer([FromBody] AddManufacturerSellDto manufacturerSellDto)
        {
            return Request.ExecuteProtectedAndWrapResult<AddManufacturerSellDto, PaymentInfo>(
                dto => TradeService.CreateManufacturerTradePromise(dto, $"http://{Request.RequestUri.Authority}/api/Trade/ConfirmOrderFromManufacturer"),
                ModelState, manufacturerSellDto
            );
        }

        [HttpPost]
        public HttpResponseMessage ConfirmOrderFromManufacturer(PaymentConfirmDto paymentConfirmDto)
        {
            return Request.ExecuteProtectedAndWrapResult<PaymentConfirmDto, ManufacturerSellModel>(
                dto => TradeService.ConfirmManufacturerTradePromise(dto),
                ModelState, paymentConfirmDto
            );
        }
    }
}
