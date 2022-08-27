using FlatsAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlatsAPI.Services.Scheduled;

public class RentHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RentHostedService> _logger;

    public RentHostedService(IServiceScopeFactory scopeFactory, ILogger<RentHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rent Hosted Service Running");

        await GenerateRents(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rent Hosted Service stopped");

        await base.StopAsync(cancellationToken);
    }

    public async Task GenerateRents(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generating rents...");
        using (var scope = _scopeFactory.CreateScope())
        {

            var dbContext = scope.ServiceProvider.GetRequiredService<FlatsDbContext>();

            var rentContext = scope.ServiceProvider.GetRequiredService<IRentService>();

            //using var context = new FlatsDbContext();

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var account in dbContext.Accounts.ToList())
                {
                    await rentContext.GenerateRentsForOwnerByIdAsync(account.Id, cancellationToken);
                    await rentContext.AddTenantRentsAsync(account.Id, cancellationToken);
                }
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
