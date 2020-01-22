using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineRoulette.Api.SignalrHubs;
using OnlineRoulette.Application.Commands;
using OnlineRoulette.Application.Common.Dtos;
using OnlineRoulette.Application.Queries;
using OnlineRoulette.Domain.Entities;
using System.Threading.Tasks;

namespace OnlineRoulette.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + nameof(Startup.v1) + "/[controller]")]
    public class BetController : ApiController
    {
        private readonly IHubContext<JackpotNotificationHub> _hubContext;

        public BetController(IHubContext<JackpotNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        #region Commands

        /// <summary>
        /// Make new bet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("makeBet")]
        public async Task<ActionResult<BetDto>> MakeBet(MakeBetCommand command)
        {
            var responce = await Mediator.Send(command);

            await _hubContext.Clients.All.SendAsync("jackpotAmountChanged", $"Jackpot amount has increased: {await Mediator.Send(new JackpotQuery())}");

            return responce;
        }

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
        /// Change spin status for ex: Closed, failed and etc..
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("changeSpinStatus")]
        public async Task<ActionResult<Unit>> ChangeSpinStatus(SpinStatusChangeCommand command)
            => await Mediator.Send(command);

        #endregion

        #region Queries

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