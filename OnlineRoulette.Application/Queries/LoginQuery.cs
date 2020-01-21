using MediatR;

namespace OnlineRoulette.Application.Queries
{

    /// <summary>
    /// LogIn user 
    /// </summary>
    public class LoginQuery : IRequest<string>
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
    }
}
