using Microsoft.AspNetCore.Http;
using OnlineRoulette.Application.Common.Interfaces;
using System.Security.Claims;

namespace OnlineRoulette.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name), out int userId);
            CurrentUserId = userId;

            IpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public int CurrentUserId { get; }
        public string IpAddress { get; }
    }
}
