namespace Services.Pin
{
    public class ESP8266PinService : PinService
    {
        public override bool IsValidIndicatorPin(int number)
        {
            return number >= 1 && number <= 12 && number != 3;
        }

        public override bool IsValidReaderPin(int number)
        {
            return number >= 1 && number <= 12 && number != 3 && number != 4;
        }
    }
}
