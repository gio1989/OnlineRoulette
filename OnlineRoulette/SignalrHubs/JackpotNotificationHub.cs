using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OnlineRoulette.Api.SignalrHubs
{
    public class JackpotNotificationHub : Hub
    {
        public async Task SendJackpotChangedNotification(string jackpotAmount)
        {
            await Clients.All.SendAsync("jackpotAmountChanged", jackpotAmount);
        }
    }
}
