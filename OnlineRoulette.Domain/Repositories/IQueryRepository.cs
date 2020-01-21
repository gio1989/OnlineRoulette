using OnlineRoulette.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRoulette.Domain.Repositories
{
    public interface IQueryRepository
    {
        Task<UserEntity> FindUserByUserName(string userName);
        Task<UserEntity> FindUserById(int id);
        Task<decimal> GetCurrentJackpot();
        Task<int> GetNumberOfBets(int userId);
        Task<decimal?> GetUserBalance(int userId);
        Task<IEnumerable<BetEntity>> GetUserBetHistory(int userId, int? pageNumber, int? itemsPerPage);
        Task<int> GetBetHistoryCount(int userId);
    }
}
