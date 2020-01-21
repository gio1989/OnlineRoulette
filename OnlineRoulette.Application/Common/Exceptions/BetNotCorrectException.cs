using System.Net;

namespace OnlineRoulette.Application.Common.Exceptions
{
    public class BetNotCorrectException : BaseApiException
    {
        public override string Message { get; }

        public BetNotCorrectException() : base(HttpStatusCode.BadRequest)
        {
            Message = "Bet not correct";
        }

        public BetNotCorrectException(string message) : base(HttpStatusCode.BadRequest)
        {
            Message = message;
        }
    }
}
