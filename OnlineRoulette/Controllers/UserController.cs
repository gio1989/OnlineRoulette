using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineRoulette.Api.Controllers;
using OnlineRoulette.Api.SignalrHubs;
using OnlineRoulette.Application.Commands;
using OnlineRoulette.Application.Common.Dtos;
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
        private readonly IHubContext<JackpotNotificationHub> _hubContext;

        public UserController(IHubContext<JackpotNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

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

        /// <summary>
        /// Make new bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("makeBet")]
        public async Task<ActionResult<BetDto>> MakeBet(MakeBetCommand command)
        {
            await _hubContext.Clients.All.SendAsync("jackpotAmountChanged", $"Jackpot amount has increased: {await Mediator.Send(new JackpotQuery())}");
            return await Mediator.Send(command);
        }

        #endregion

        #region Queries

        /// <summary>
        /// Login to the system. 
        /// email: test@test.net
        /// pwd: test
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
