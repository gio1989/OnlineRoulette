using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Enums;
using OnlineRoulette.Domain.Repositories;
using OnlineRoulette.Infrastructure.DBProvider;
using System;
using System.Threading.Tasks;

namespace OnlineRoulette.Infrastructure.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly IDBProvider _dbProvider;

        public CommandRepository(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task MakeBet(BetEntity bet, decimal balance, int winningNumber)
        {

            var db = _dbProvider.GetDBInstance();

            try
            {
                db.Open();
                db.BeginTransaction();

                await UpdateSpin(bet.SpinId, winningNumber, db);

                await UpdateBalance(bet.UserId, balance, db);

                await CreateBet(bet, db);

                await UpdateJackpot(bet.JackpotAmount, db);

                db.CommitTransaction();
            }
            catch
            {
                db.RollBackTransaction();
            }
            finally
            {
                db.Close();
                db.Dispose();
            }
        }

        public async Task<long> CreateSpin(SpinEntity spin)
        {
            long insertedId = default;

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                var createSpinSqlCmd = @"INSERT INTO spins(
                                       SpinStatus, WinningNumber, CreatedAt)
                                       VALUES(@SpinStatus, @WinningNumber, @CreatedAt);
                                       select LAST_INSERT_ID()";

                insertedId = await db.ExecuteScalarAsync(createSpinSqlCmd, new
                {
                    spin.SpinStatus,
                    spin.WinningNumber,
                    CreatedAt = DateTime.Now,
                });

                db.Close();
            }

            return insertedId;
        }

        /// <summary>
        /// Change spin Status 
        /// </summary>
        /// <param name="spinId"></param>
        /// <param name="spinStatus"></param>
        /// <returns></returns>
        public async Task ChangeSpinStatus(int spinId, SpinStatus spinStatus)
        {

            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                var changeSpinStatusSqlCmd = @"update spins
                                            set UpdatedAt=@UpdatedAt,
                                            SpinStatus=@SpinStatus
                                            where id=@Id";

                await db.ExecuteAsync(changeSpinStatusSqlCmd, new
                {
                    UpdatedAt = DateTime.Now,
                    SpinStatus = (byte)spinStatus,
                    id = spinId
                });

                db.Close();
            }
        }


        public async Task<SpinEntity> SpinById(int id)
        {
            using (var db = _dbProvider.GetDBInstance())
            {
                db.Open();

                string spinByIdQuery = @" select * from spins
                                           where id=1";

                return await db.QueryFirstOrDefaultAsync<SpinEntity>(spinByIdQuery, new { id });
            }
        }

        #region Private Methods

        private async Task UpdateSpin(int spinId, int winningNumber, Database db)
        {
            var updateSpinSqlCmd = @" update spins
                                            set UpdatedAt=@UpdatedAt,
                                            WinningNumber=@WinningNumber
                                            where id=@Id";

            await db.ExecuteAsync(updateSpinSqlCmd, new
            {
                UpdatedAt = DateTime.Now,
                winningNumber,
                id = spinId
            });
        }

        public async Task UpdateBalance(int userId, decimal balance, Database db)
        {
            var updateBalanceSqlCmd = @"UPDATE Users SET 
                                            Balance = @Balance,
                                            UpdatedAt = @UpdatedAt
                                            WHERE Id = @Id";

            await db.ExecuteAsync(updateBalanceSqlCmd, new
            {
                Balance = balance,
                UpdatedAt = DateTime.Now,
                Id = userId
            });
        }

        public async Task CreateBet(BetEntity bet, Database db)
        {
            var createBetSqlCmd = @"INSERT INTO bets(SpinId,
                                       UserId, BetString, BetAmount,JackpotAmount,
                                       WonAmount, BetStatus, IpAddress, CreatedAt)
                                       VALUES(@SpinId, @UserId, @BetString, @BetAmount, 
                                       @JackpotAmount, @WonAmount, @BetStatus,
                                       @IpAddress, @CreatedAt)";

            await db.ExecuteAsync(createBetSqlCmd, new
            {
                bet.SpinId,
                bet.UserId,
                bet.BetString,
                bet.BetAmount,
                bet.JackpotAmount,
                bet.WonAmount,
                BetStatus = (byte)bet.BetStatus,
                bet.IpAddress,
                CreatedAt = DateTime.Now
            });
        }

        private async Task UpdateJackpot(decimal? jackpotAmount, Database db)
        {
            var updateJackPot = @"UPDATE JackPot SET
                        Amount = @JackPotAmount,
                        UpdatedAt = @UpdatedAt";

            await db.ExecuteAsync(updateJackPot, new
            {
                JackPotAmount = jackpotAmount,
                UpdatedAt = DateTime.Now
            });
        }


        #endregion
    }
}
