using Models;

namespace Services.Pin
{
    public class ESP8266PinService : PinService
    {
        public override bool IsValidConnectionHelperPin(int number)
        {
            return number >= 1 && number <= 12;
        }
    }
}
