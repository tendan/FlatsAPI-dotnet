using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IAccountService
    {
        void CreateAccount(CreateAccountDto dto);
        string GenerateJwt(LoginDto dto);
        AccountDto GetSpecifiedAccountByEmail(string email);
        ICollection<RentDto> GetSpecifiedAccountRentsByEmail(string email);
        void DeleteAccountById(int id);
    }
    public class AccountService : IAccountService
    {
        public AccountService() { }

        public void CreateAccount(CreateAccountDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccountById(int id)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwt(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public AccountDto GetSpecifiedAccountByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ICollection<RentDto> GetSpecifiedAccountRentsByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
