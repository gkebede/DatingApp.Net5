using DatingApp_API.interfaces;
using DatingApp_API.Models;
using DatingApp_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                    IConfiguration config) 
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContextPool<DataContext>((optons) => {
                optons.UseSqlServer(config.GetConnectionString("DatingAppDBConnection"));
            });
            return services;
        }
    }
}
