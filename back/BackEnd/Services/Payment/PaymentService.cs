﻿using Models.Trade;

namespace Services.Payment
{
    public abstract class PaymentService
    {
        public abstract PaymentInfo CreatePaymentToManufacturer(PaymentPrepareModel paymentPrepare);
        public abstract bool IsPaymentAuthorized(PaymentInfo payment);
        public abstract string GetOrderId(PaymentInfo payment);
    }
}