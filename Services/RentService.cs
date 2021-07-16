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
        Task AddTenantRentsAsync(int tenantId, CancellationToken cancellationToken);
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

        public async Task AddTenantRentsAsync(int tenantId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Generating rents for tenant with ID: {tenantId}");

            var account = _dbContext.Accounts
                .Include(a => a.RentedFlats)
                .Include(a => a.Rents)
                .FirstOrDefault(a => a.Id == tenantId);

            if (account is null)
                throw new NotFoundException("Account not found");

            var rentedFlats = account.RentedFlats.ToList();

            foreach (var rentedFlat in rentedFlats)
            {
                var rentsForTenant = rentedFlat.Rents.ToList();

                var flatId = rentedFlat.Id;

                var priceWhenRented = rentedFlat.PricePerMeterSquaredWhenRented ?? (rentedFlat.PriceWhenBought / PaymentSettings.RENTED_SPLIT_UP);

                if (rentsForTenant.Any())
                {
                    rentsForTenant.ToList().ForEach(async r =>
                    {
                        if (r.PayDate.CompareTo(DateTime.Now) <= 0 && !r.Paid)
                        {
                            var newRent = AddRent(priceWhenRented, flatId, account, PropertyTypes.Flat, OwnerShip.RENTED);

                            rentedFlat.Rents.Add(newRent);

                            await _dbContext.Rents.AddAsync(newRent);

                            _dbContext.Update(rentedFlat);
                        }
                    });
                } 
                else
                {
                    var newRent = AddRent(priceWhenRented, flatId, account, PropertyTypes.Flat, OwnerShip.RENTED);

                    rentedFlat.Rents.Add(newRent);

                    await _dbContext.Rents.AddAsync(newRent);

                    _dbContext.Update(rentedFlat);
                }
            }

            await _dbContext.SaveChangesAsync();

            await Task.Delay(5000, cancellationToken);
        }

        public async Task GenerateRentsForOwnerByIdAsync(int accountId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Generating owner's rents for account with ID: {accountId}");

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

                var flatId = ownedFlat.Id;

                var priceWhenBought = ownedFlat.PriceWhenBought;

                if (rentExists) { continue; }

                var newRent = AddRent(priceWhenBought,
                    flatId,
                    account,
                    PropertyTypes.Flat,
                    OwnerShip.BOUGHT);

                ownedFlat.Rents.Add(newRent);

                await _dbContext.Rents.AddAsync(newRent);

                _dbContext.Update(ownedFlat);
            }

            foreach (var ownedBlockOfFlats in account.OwnedBlocksOfFlats.ToList())
            {
                var rentExists = rentsForAccount.Where(r => r.OwnerShip == OwnerShip.BOUGHT &&
                r.PropertyType == PropertyTypes.BlockOfFlats &&
                r.PropertyId == ownedBlockOfFlats.Id).Any();

                var blockOfFlatsId = ownedBlockOfFlats.Id;

                var price = ownedBlockOfFlats.Price;

                if (rentExists) { continue; }

                var newRent = AddRent(price,
                    blockOfFlatsId,
                    account,
                    PropertyTypes.BlockOfFlats,
                    OwnerShip.BOUGHT);

                ownedBlockOfFlats.Rents.Add(newRent);

                await _dbContext.Rents.AddAsync(newRent);

                _dbContext.Update(ownedBlockOfFlats);
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
            var rent = AddRentWithCustomPayDate(price, propertyId, account, DateTime.Now.AddDays(30), propertyType, ownerShip);

            return rent;
        }

        private Rent AddRentWithCustomPayDate(float price, int propertyId, Account account, DateTime payDate, PropertyTypes propertyType, OwnerShip ownerShip)
        {
            var rent = new Rent()
            {
                CreationDate = DateTime.Now,
                PayDate = payDate,
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
