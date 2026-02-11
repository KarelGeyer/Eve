using Common.Shared.Interfaces;
using Eve.API.ServiceAccessors;
using Users.Application;
using Users.Application.Interfaces;
using Users.Infrastructure;

namespace Eve.API.Extensions
{
    public static class ModulesDependencyInjection
    {
        public static IServiceCollection AddModulesServices(this IServiceCollection services, IConfiguration configuration)
        {
            // USERS MODULE
            services.AddUserApplication();
            services.AddUserInfrastructure(configuration);

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IDeviceDetector, DeviceDetector>();

            // GROUPS MODULE
            return services;
        }
    }
}
