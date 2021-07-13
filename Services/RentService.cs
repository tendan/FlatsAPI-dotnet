using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Settings;
using FlatsAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FlatsAPI.Services
{
    public interface IRentService
    {
        Task GenerateRentsForOwnerByIdAsync(int accountId, CancellationToken cancellationToken);
        Task AddTenantRentsAsync(int flatId);
        public void DeleteUnpaidRents();
    }
    public class RentService : IRentService
    {
        private readonly FlatsDbContext _dbContext;
        private readonly ILogger<RentService> _logger;

        public RentService(FlatsDbContext dbContext, ILogger<RentService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddTenantRentsAsync(int flatId)
        {
            var flat = _dbContext.Flats.FirstOrDefault(f => f.Id == flatId);

            if (flat is null)
                throw new NotFoundException("Flat not found");

            var tenants = flat.Tenants;

            foreach (var tenant in tenants)
            {
                var priceWhenRentedIfNotDefined = flat.PriceWhenBought / PaymentSettings.RENTED_SPLIT_UP;
                AddRent(flat.PricePerMeterSquaredWhenRented ?? priceWhenRentedIfNotDefined, flat.Id, tenant, PropertyTypes.Flat, OwnerShip.RENTED);
            };
            _dbContext.SaveChanges();
        }

        public async Task GenerateRentsForOwnerByIdAsync(int accountId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Generating rents for account with ID: {accountId}");
            var account = _dbContext.Accounts
                .Include(a => a.OwnedFlats)
                .Include(a => a.OwnedBlocksOfFlats)
                .FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");

            var rentsForAccount = _dbContext.Rents.Where(r => r.RentIssuer == account);

            foreach (var ownedFlat in account.OwnedFlats.ToList())
            {
                var rentExists = rentsForAccount.Where(r => r.OwnerShip == OwnerShip.BOUGHT &&
                r.PropertyType == PropertyTypes.Flat &&
                r.PropertyId == ownedFlat.Id).Any();

                if (rentExists) { continue; }

                var newRent = AddRent(ownedFlat.PriceWhenBought,
                    ownedFlat.Id,
                    account,
                    PropertyTypes.Flat,
                    OwnerShip.BOUGHT);

                account.Rents.Add(newRent);
                _dbContext.Rents.Add(newRent);
                _dbContext.Update(account);
            }

            foreach (var ownedBlockOfFlats in account.OwnedBlocksOfFlats.ToList())
            {
                var rentExists = rentsForAccount.Where(r => r.OwnerShip == OwnerShip.BOUGHT &&
                r.PropertyType == PropertyTypes.BlockOfFlats &&
                r.PropertyId == ownedBlockOfFlats.Id).Any();

                if (rentExists) { continue; }

                var newRent = AddRent(ownedBlockOfFlats.Price,
                    ownedBlockOfFlats.Id,
                    account,
                    PropertyTypes.BlockOfFlats,
                    OwnerShip.BOUGHT);

                account.Rents.Add(newRent);
                _dbContext.Rents.Add(newRent);
                _dbContext.Update(account);
            }

            await _dbContext.SaveChangesAsync();

            await Task.Delay(5000, cancellationToken);
        }

        public void DeleteUnpaidRents()
        {
            _dbContext.Rents.RemoveRange(_dbContext.Rents.Where(r => !r.Paid));
            _dbContext.SaveChanges();
        }

        private Rent AddRent(float price, int propertyId, Account account, PropertyTypes propertyType, OwnerShip ownerShip)
        {
            var rent = new Rent()
            {
                CreationDate = DateTime.Now,
                PayDate = DateTime.Now.AddDays(30),
                Paid = false,
                Price = price,
                PriceWithTax = price * PaymentSettings.TAX,
                PropertyType = propertyType,
                PropertyId = propertyId,
                RentIssuer = account,
                OwnerShip = ownerShip
            };

            return rent;
        }
    }
}
