using Newtonsoft.Json;

namespace ServicesContract.Exceptions
{
    public class ForbiddenException : CustomException
    {
        [JsonProperty("needed_rights_level")]
        private string NeededRightsLevel { get; set; }

        public override string Message => $"You should be at least {NeededRightsLevel} to do this";

        public ForbiddenException(string neededRightsLevel)
        {
            NeededRightsLevel = neededRightsLevel;
        }
    }
}
