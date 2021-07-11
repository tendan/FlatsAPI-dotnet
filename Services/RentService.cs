using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Settings;

namespace FlatsAPI.Services
{
    public interface IRentService
    {
        void GenerateRentsForOwnerById(int accountId);
        void AddTenantRents(int flatId);
    }
    public class RentService : IRentService
    {
        private readonly FlatsDbContext _dbContext;

        public RentService(FlatsDbContext dbContext)
        {
            _dbContext = dbContext;
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
                AddRent(flat.PricePerMeterSquaredWhenRented ?? priceWhenRentedIfNotDefined, flat.Id, tenant, PropertyTypes.Flat);
            };
            _dbContext.SaveChanges();
        }

        public void GenerateRentsForOwnerById(int accountId)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");
            
            foreach (var ownedFlat in account.OwnedFlats)
            {
                AddRent(ownedFlat.PriceWhenBought, ownedFlat.Id, account, PropertyTypes.Flat);
            }

            foreach (var ownedBlockOfFlats in account.OwnedBlocksOfFlats)
            {
                AddRent(ownedBlockOfFlats.Price, ownedBlockOfFlats.Id, account, PropertyTypes.BlockOfFlats);
            }
            _dbContext.SaveChanges();
        }

        private void AddRent(float price, int propertyId, Account account, PropertyTypes propertyType)
        {
            var rent = new Rent()
            {
                CreationDate = DateTime.Now,
                PayDate = DateTime.Now.AddDays(30),
                Paid = false,
                Price = price,
                PriceWithTax = price * PaymentSettings.TAX,
                Property = propertyType,
                PropertyId = propertyId,
                RentIssuer = account
            };

            _dbContext.Rents.Add(rent);
        }
    }
}
