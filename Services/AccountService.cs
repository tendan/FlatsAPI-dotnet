using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IAccountService
    {
        void Create(CreateAccountDto dto);
        string GenerateJwt(LoginDto dto);
        AccountDto GetByEmail(string email);
        ICollection<RentDto> GetRentsByEmail(string email);
        void DeleteById(int id);
    }
    public class AccountService : IAccountService
    {
        public AccountService() { }

        public void Create(CreateAccountDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwt(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public AccountDto GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ICollection<RentDto> GetRentsByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
