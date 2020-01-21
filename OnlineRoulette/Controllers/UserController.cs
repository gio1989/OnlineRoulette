using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRoulette.Api.Controllers;
using OnlineRoulette.Application.Commands;
using OnlineRoulette.Application.Queries;
using OnlineRoulette.Domain.Entities;
using System.Threading.Tasks;

namespace OnlineRoulette.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + nameof(Startup.v1) + "/[controller]")]
    public class UserController : ApiController
    {
        #region Commands

        /// <summary>
        /// Create spin before begin betting.
        /// Returned Id must be saved in client app 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("createSpin")]
        public async Task<ActionResult<long>> CreateSpin()
            => await Mediator.Send(new CreateSpinCommand());

        [AllowAnonymous]
        [HttpPost("closeSpin")]
        public async Task<ActionResult<long>> CloseSpin()
           => await Mediator.Send(new CreateSpinCommand()); 

        /// <summary>
        /// Make new bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("makeBet")]
        public async Task<ActionResult<BetEntity>> MakeBet(MakeBetCommand command)
            => await Mediator.Send(command);

        #endregion

        #region Queries

        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogIn(LoginQuery query)
           => await Mediator.Send(query);

        /// <summary>
        /// Get current jackpot amount
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentJackpot")]
        public async Task<ActionResult<decimal>> GetCurrentJackpot()
            => await Mediator.Send(new JackpotQuery());

        /// <summary>
        /// Get bet history by current user
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("betHistory")]
        public async Task<ActionResult<PagedData<BetEntity>>> GetBetHistory(BetHistoryQuery query)
            => await Mediator.Send(query);

        #endregion

    }
}
