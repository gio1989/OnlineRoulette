using MediatR;
using OnlineRoulette.Domain.Entities;

namespace OnlineRoulette.Application.Commands
{
    /// <summary>
    /// To create a bet 
    /// </summary>
    public class MakeBetCommand : IRequest<BetEntity>
    {
        
        /// <summary>
        /// BetString  in json format
        /// </summary>
        public dynamic BetString { get; set; }
        /// <summary>
        /// beted Amount 
        /// </summary>
        public decimal BetAmount { get; set; }

        /// <summary>
        /// Generating when begin betting
        /// </summary>
        public int SpinId { get; set; }

    }
}
