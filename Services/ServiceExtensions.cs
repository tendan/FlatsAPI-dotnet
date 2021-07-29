using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IBlockOfFlatsService, BlockOfFlatsService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddTransient<IRentService, RentService>();

            return services;
        }
    }
}
