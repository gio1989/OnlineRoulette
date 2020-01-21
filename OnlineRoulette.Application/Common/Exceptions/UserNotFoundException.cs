using System.Net;

namespace OnlineRoulette.Application.Common.Exceptions
{
    public class UserNotFoundException : BaseApiException
    {
        public override string Message { get; }

        public UserNotFoundException() : base(HttpStatusCode.NotFound)
        {
            Message = "User not found";
        }
    }
}