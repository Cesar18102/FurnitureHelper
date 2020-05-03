using Newtonsoft.Json;

namespace ServicesContract.Exceptions
{
    public class NotFoundException : CustomException
    {
        [JsonProperty("not_found")]
        private string NotFoundSubject { get; set; }

        public override string Message => $"{NotFoundSubject} not found";

        public NotFoundException(string notFoundSubject)
        {
            NotFoundSubject = notFoundSubject;
        }
    }
}
