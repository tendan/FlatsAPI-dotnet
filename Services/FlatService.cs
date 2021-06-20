using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IFlatService
    {
        void CreateFlat(CreateFlatDto dto);
        ICollection<FlatDto> GetAllFlats();
        FlatDto GetSpecifiedFlatById(int id);
        ICollection<RentDto> GetSpecifiedFlatRentsByFlatsId(int id);
        void ApplyTenantForFlatByIds(int flatId, int tenantId, OwnerShip ownerShip);
        void DeleteFlatById(int id);
    }
    public class FlatService : IFlatService
    {
        public FlatService() { }
        public void ApplyTenantForFlatByIds(int flatId, int tenantId, OwnerShip ownerShip)
        {
            throw new NotImplementedException();
        }

        public void CreateFlat(CreateFlatDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteFlatById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<FlatDto> GetAllFlats()
        {
            throw new NotImplementedException();
        }

        public FlatDto GetSpecifiedFlatById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<RentDto> GetSpecifiedFlatRentsByFlatsId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
