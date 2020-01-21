using OnlineRoulette.Domain.Entities;
using System.Threading.Tasks;

namespace OnlineRoulette.Domain.Repositories
{
    public interface ICommandRepository
    {
        Task MakeBet(BetEntity bet, decimal balance, int winningNumber);
        Task<long> CreateSpin(SpinEntity spin);
    }
}
