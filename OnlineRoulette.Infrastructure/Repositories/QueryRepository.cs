using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Repositories;
using OnlineRoulette.Infrastructure.DBProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRoulette.Infrastructure.Repositories
{

    public class QueryRepository : IQueryRepository
    {

        private readonly IDBProvider _dbProvider;

        public QueryRepository(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Retrive current jackpot amount
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetCurrentJackpot()
        {
            string sql = @"SELECT Amount FROM 
                            Jackpot WHERE IsActive = 1";

            decimal jackPot = default;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                jackPot = await db.QueryFirstOrDefaultAsync<decimal>(sql);

                db.Close();
            }

            return jackPot;
        }

        /// <summary>
        /// Get current user's balance
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<decimal?> GetUserBalance(int userId)
        {
            string sql = @"SELECT Balance FROM Users
                            WHERE Id = @userId";

            UserEntity user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { userId });

                db.Close();
            }

            return user?.Balance;
        }

        /// <summary>
        /// Retrive user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UserEntity> FindUserByUserName(string userName)
        {
            string sql = @"SELECT * FROM Users
                           WHERE Email = @userName";

            UserEntity user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { userName });

                db.Close();
            }

            return user;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserEntity> FindUserById(int id)
        {
            string sql = @"SELECT * FROM Users
                           WHERE Id = @Id";

            UserEntity user = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                user = await db.QueryFirstOrDefaultAsync<UserEntity>(sql, new { id });

                db.Close();
            }

            return user;
        }

        /// <summary>
        /// Get number of bets for paging
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetNumberOfBets(int userId)
        {
            string sql = "SELECT COUNT(1) FROM Bets WHERE UserId = @userId";

            int numberOfBets = default;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                numberOfBets = await db.ExecuteScalarAsync(sql, new { userId });

                db.Close();
            }
            return numberOfBets;
        }

        /// <summary>
        /// Get Bet history by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BetEntity>> GetUserBetHistory(int userId, int? pageNumber, int? take)
        {
            var page = pageNumber == null ? 0 : (pageNumber - 1) * take;
            var itemsPerPage = take == null ? 10 : take;

            IEnumerable<BetEntity> bets = null;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                UserEntity user = await GetUserById(userId, db);

                if (user != null)
                    bets = await GetBetHistory(userId, page, itemsPerPage, db);

                db.Close();
            }

            return bets;
        }

        public async Task<int> GetBetHistoryCount(int userId)
        {
            string betHistoryCountQuery = "SELECT COUNT(1) FROM Bets WHERE UserId = @userId";

            int numberOfBets = default;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                numberOfBets = await db.ExecuteScalarAsync(betHistoryCountQuery, new { userId });

                db.Close();
            }
            return numberOfBets;
        }


        #region Private Methods

        private async Task<UserEntity> GetUserById(int id, Database db)
        {
            string userQuery = @"SELECT Id FROM Users 
                                        WHERE Id = @id";

            return await db.QueryFirstOrDefaultAsync<UserEntity>(userQuery, new { id });
        }

        private async Task<IEnumerable<BetEntity>> GetBetHistory(int userId, int? pageNumber, int? itemsPerPage, Database db)
        {
            string betQuery = @"SELECT SpinID, BetAmount, BetString, BetStatus, CreatedAt
                                      FROM bets WHERE UserId = @userId
                                      ORDER BY CreatedAt DESC 
                                      LIMIT @pageNumber, @itemsPerPage";

            return await db.QueryAsync<BetEntity>(betQuery, new { userId, pageNumber, itemsPerPage });
        }
        #endregion

    }
}