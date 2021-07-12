using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using FlatsAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FlatsAPI.Services.Scheduled
{
    public class RentHostedService : IHostedService
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RentHostedService> _logger;

        public RentHostedService(IServiceScopeFactory scopeFactory, ILogger<RentHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(GenerateRents, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void GenerateRents(object state)
        {
            _logger.LogInformation("Generating rents...");
            using (var scope = _scopeFactory.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetRequiredService<FlatsDbContext>();

                var rentContext = scope.ServiceProvider.GetRequiredService<IRentService>();

                //using var context = new FlatsDbContext();

                foreach (var account in dbContext.Accounts.ToList())
                {
                    rentContext.GenerateRentsForOwnerById(account.Id);
                }
            }
        }
    }
}
