using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IFlatService
    {
        void Create(CreateFlatDto dto);
        ICollection<FlatDto> GetAll();
        FlatDto GetById(int id);
        ICollection<RentDto> GetRentsById(int id);
        void ApplyTenantByIds(int flatId, int tenantId, OwnerShip ownerShip);
        void Delete(int id);
    }
    public class FlatService : IFlatService
    {
        public FlatService() { }
        public void ApplyTenantByIds(int flatId, int tenantId, OwnerShip ownerShip)
        {
            throw new NotImplementedException();
        }

        public void Create(CreateFlatDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<FlatDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public FlatDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<RentDto> GetRentsById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
