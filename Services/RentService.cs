using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Settings;
using FlatsAPI.Models;
using Microsoft.Extensions.Logging;

namespace FlatsAPI.Services
{
    public interface IRentService
    {
        List<Rent> GenerateRentsForOwnerById(int accountId);
        void AddTenantRents(int flatId);
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

        public void AddTenantRents(int flatId)
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

        public List<Rent> GenerateRentsForOwnerById(int accountId)
        {
            _logger.LogInformation($"Generating rents for account with ID: {accountId}");
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");

            var rents = new List<Rent>();

            var ownedFlats = _dbContext.Flats.Where(f => f.Owner == account);
            
            foreach (var ownedFlat in ownedFlats)
            {
                Console.WriteLine(ownedFlat.ToString());
                if (ownedFlat.Rents.Any(r => r.Paid && r.OwnerShip == OwnerShip.BOUGHT && r.RentIssuer == account)) continue;
                var rent = AddRent(ownedFlat.PriceWhenBought, ownedFlat.Id, account, PropertyTypes.Flat, OwnerShip.BOUGHT);
                ownedFlat.Rents.Add(rent);
                rents.Add(rent);
            }

            var ownedBlocksOfFlats = _dbContext.BlockOfFlats.Where(b => b.Owner == account);

            foreach (var ownedBlockOfFlats in ownedBlocksOfFlats)
            {
                if (ownedBlockOfFlats.Rents.Any(r => r.Paid && r.OwnerShip == OwnerShip.BOUGHT && r.RentIssuer == account)) continue;
                var rent = AddRent(ownedBlockOfFlats.Price, ownedBlockOfFlats.Id, account, PropertyTypes.BlockOfFlats, OwnerShip.BOUGHT);
                ownedBlockOfFlats.Rents.Add(rent);
                rents.Add(rent);
            }
            _dbContext.SaveChanges();

            return rents;
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

            _dbContext.Rents.Add(rent);

            return rent;
        }
    }
}
