using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

using Models.Trade;

namespace Services.Payment
{
    public class LiqPayPaymentService : PaymentService
    {
        private class LiqPayPaymentInfo
        {
            [JsonProperty("version")]
            public int Version { get; set; }

            [JsonProperty("public_key")]
            public string PublicKey { get; set; }

            [JsonIgnore]
            public string PrivateKey { get; set; }

            [JsonProperty("action")]
            public string Action { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("amount")]
            public float Amount { get; private set; }

            [JsonProperty("order_id")]
            public string OrderId { get; private set; }

            [JsonProperty("description")]
            public string Description { get; private set; }

            [JsonProperty("server_url")]
            public string CallbackUrl { get; private set; }

            [JsonConstructor]
            public LiqPayPaymentInfo() { }

            public LiqPayPaymentInfo(PaymentPrepareModel prepare)
            {
                Amount = prepare.Amount;
                OrderId = prepare.OrderId;
                Description = prepare.Description;
                CallbackUrl = prepare.CallbackUrl;
            }
        }

        private static readonly HashAlgorithm Sha1 = SHA1.Create();

        private const string PUBLIC_KEY = "sandbox_i74310151520";
        private const string PRIVATE_KEY = "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn";

        private const int VERSION = 3;
        private const string ACTION = "pay";
        private const string CURRENCY = "UAH";

        public override PaymentInfo CreatePaymentToManufacturer(PaymentPrepareModel paymentPrepare)
        {
            LiqPayPaymentInfo liqPayPayment = new LiqPayPaymentInfo(paymentPrepare)
            {
                Version = VERSION,
                Action = ACTION,
                Currency = CURRENCY,
                PublicKey = PUBLIC_KEY,
                PrivateKey = PRIVATE_KEY,
            };

            string json = JsonConvert.SerializeObject(liqPayPayment);
            byte[] dataBytes = Encoding.UTF8.GetBytes(json);
            string data = Convert.ToBase64String(dataBytes);
            string signature = GetSignature(data);
            
            return new PaymentInfo(data, signature);
        }

        private string GetSignature(string data)
        {
            string sign = PRIVATE_KEY + data + PRIVATE_KEY;
            byte[] signBytes = Encoding.UTF8.GetBytes(sign);
            byte[] hash = Sha1.ComputeHash(signBytes);
            return Convert.ToBase64String(hash);
        }

        public override bool IsPaymentAuthorized(PaymentInfo payment)
        {
            string calculatedSignature = GetSignature(payment.Data);
            return calculatedSignature == payment.Signature;
        }

        public override string GetOrderId(PaymentInfo payment)
        {
            byte[] jsonBytes = Convert.FromBase64String(payment.Data);
            string json = Encoding.UTF8.GetString(jsonBytes);

            LiqPayPaymentInfo liqPayPayment = JsonConvert.DeserializeObject<LiqPayPaymentInfo>(json);
            return liqPayPayment.OrderId;
        }
    }
}
