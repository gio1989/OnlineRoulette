using System.Net;

namespace OnlineRoulette.Application.Common.Exceptions
{
    public class EmailNotValidException : BaseApiException
    {
        public override string Message { get; }

        public EmailNotValidException() : base(HttpStatusCode.BadRequest)
        {
            Message = "Email is not valid";
        }

        public EmailNotValidException(string message) : base(HttpStatusCode.BadRequest)
        {
            Message = message;
        }
    }
}
