namespace ServicesContract.Dto
{
    public class PaymentConfirmDto : IDto
    {
        public string data { get; set; }
        public string signature { get; set; }

        public void Validate()
        {
        }
    }
}
