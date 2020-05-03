namespace ServicesContract.Exceptions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException(string message = "Unauthorized")
        {
            msg = message;
        }

        private string msg { get; set; }
        public override string Message => msg;
    }
}
