using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineRoulette.Domain.Repositories;
using OnlineRoulette.Infrastructure.DBProvider;
using OnlineRoulette.Infrastructure.Repositories;
using dbProv = OnlineRoulette.Infrastructure.DBProvider;

namespace OnlineRoulette.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<dbProv.IDBProvider, dbProv.DBProvider>();
            DataAccessLayerBase.ConnectionString = configuration["connectionString"];

            services.AddTransient<IQueryRepository, QueryRepository>();
            services.AddTransient<ICommandRepository, CommandRepository>();

            return services;
        }
    }
}
