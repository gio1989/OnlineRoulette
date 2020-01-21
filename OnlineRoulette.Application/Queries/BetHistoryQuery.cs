using MediatR;
using OnlineRoulette.Domain.Entities;

namespace OnlineRoulette.Application.Queries
{

    /// <summary>
    /// Get bet history by loggined user
    /// </summary>
    public class BetHistoryQuery : IRequest<PagedData<BetEntity>>
    {
        /// <summary>
        /// page number
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Items to get on page
        /// </summary>
        public int ItemsPerPage { get; set; }
    }
}
