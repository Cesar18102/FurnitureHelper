using System;

namespace Models.Trade
{
    public class PaymentPrepareModel : IModel
    {
        public float Amount { get; private set; }
        public string OrderId { get; private set; }
        public string CallbackUrl { get; set; }
        public string Description { get; set; }
        public DateTime Expired { get; set; }

        public PaymentPrepareModel(string orderId, float amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
    }
}
