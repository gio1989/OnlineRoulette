namespace OnlineRoulette.Infrastructure.DBProvider
{
    public class DBProvider : IDBProvider
    {
        public Database GetDBInstance()
          => new Database();
    }
}
