using MediatR;

namespace OnlineRoulette.Application.Queries
{

    /// <summary>
    /// Get user current balance
    /// </summary>
    public class UserBalanceQurey : IRequest<decimal?>
    {
        /// <summary>
        /// Logined user id
        /// </summary>
        public int UserId { get; set; }
    }
}
