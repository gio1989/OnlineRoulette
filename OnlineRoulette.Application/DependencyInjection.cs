using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineRoulette.Application.Common.Behaviours;
using System.Reflection;
using Auth = OnlineRoulette.Application.AuthManager;

namespace OnlineRoulette.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<Auth.IAuthManager, Auth.AuthManager>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}
