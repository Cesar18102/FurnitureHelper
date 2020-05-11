using Models.Trade;

namespace Services.Payment
{
    public abstract class PaymentService
    {
        public abstract PaymentInfo CreateFromUserPayment(PaymentPrepareModel paymentPrepare);
        public abstract bool IsPaymentAuthorized(PaymentInfo payment);
        public abstract string GetOrderId(PaymentInfo payment);
        public abstract bool IsSucceed(PaymentInfo payment);
    }
}
