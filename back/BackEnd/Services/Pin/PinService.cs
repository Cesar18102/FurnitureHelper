namespace Services.Pin
{
    public abstract class PinService
    {
        public abstract bool IsValidReaderPin(int number);
        public abstract bool IsValidIndicatorPin(int number);
    }
}
