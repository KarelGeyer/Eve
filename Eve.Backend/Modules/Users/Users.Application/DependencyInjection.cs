using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Helpers;
using Users.Application.Interfaces;
using Users.Application.Services;

namespace Users.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
