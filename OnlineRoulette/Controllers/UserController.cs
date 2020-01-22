using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRoulette.Api.Controllers;
using OnlineRoulette.Application.Common.Dtos;
using OnlineRoulette.Application.Queries;
using System.Threading.Tasks;

namespace OnlineRoulette.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/" + nameof(Startup.v1) + "/[controller]")]
    public class UserController : ApiController
    {

        #region Commands

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
        public async Task<ActionResult<UserDto>> LogIn(LoginQuery query)
           => await Mediator.Send(query);

        /// <summary>
        /// Get current jackpot amount
        /// </summary>
        /// <returns></returns>
        [HttpGet("currentJackpot")]
        public async Task<ActionResult<decimal>> GetCurrentJackpot()
            => await Mediator.Send(new JackpotQuery());

        #endregion

    }
}
