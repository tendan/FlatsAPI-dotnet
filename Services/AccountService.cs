using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using FlatsAPI.Models;
using FlatsAPI.Settings;
using FlatsAPI.Settings.Permissions;
using FlatsAPI.Settings.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IAccountService
    {
        int Create(CreateAccountDto dto);
        string GenerateJwt(LoginDto dto);
        AccountDto GetByEmail(string email);
        AccountDto GetAccountInfo();
        void Update(int accountId, UpdateAccountDto dto);
        PagedResult<RentDto> GetRentsByEmail(SearchQuery query, string email);
        void DeleteById(int id);
    }
    public class AccountService : IAccountService
    {
        private readonly FlatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IPermissionContext _permissionContext;

        public AccountService(
            FlatsDbContext dbContext,
            IMapper mapper,
            IUserContextService userContextService,
            IPasswordHasher<Account> passwordHasher,
            AuthenticationSettings authenticationSettings,
            IPermissionContext permissionContext
            ) {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _permissionContext = permissionContext;
        }

        public int Create(CreateAccountDto dto)
        {
            var account = _mapper.Map<Account>(dto);

            account.RoleId = dto.RoleId ?? _dbContext.Roles.FirstOrDefault(r => r.Name == TenantRole.Name).Id;

            var hashedPassword = _passwordHasher.HashPassword(account, dto.Password);

            account.Password = hashedPassword;
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();

            return account.Id;
        }

        public void DeleteById(int id)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == id);

            if (account is null)
                throw new NotFoundException("Account not found");

            _userContextService.AuthorizeAccess(id, AccountPermissions.DeleteOthers);

            _dbContext.Accounts.Remove(account);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Accounts
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == dto.Username);

            if (user is null)
                throw new BadRequestException("Invalid username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid username or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public AccountDto GetByEmail(string email)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Email == email);

            if (account is null)
                throw new NotFoundException("Account not found");

            var result = _mapper.Map<AccountDto>(account);
            return result;
        }

        public void Update(int accountId, UpdateAccountDto dto)
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);

            if (account is null)
                throw new NotFoundException("Account not found");

            _userContextService.AuthorizeAccess(accountId, AccountPermissions.UpdateOthers);

            account.Email = dto.Email ?? account.Email;
            account.Username = dto.Username ?? account.Username;
            account.Password = dto.Password ?? account.Password;
            account.FirstName = dto.FirstName ?? account.FirstName;
            account.LastName = dto.LastName ?? account.LastName;
            account.BillingAddress = dto.BillingAddress ?? account.BillingAddress;
            account.PhoneNumber = dto.PhoneNumber ?? account.PhoneNumber;

            _dbContext.Update(account);
            _dbContext.SaveChanges();
        }

        public PagedResult<RentDto> GetRentsByEmail(SearchQuery query, string email)
        {
            var baseQuery = _dbContext.Rents
                .Include(r => r.RentIssuer)
                .Include(r => r.PropertyId)
                .Include(r => r.PropertyType)
                .Where(r => (r.RentIssuer.Email == email))
                .Where(r => query.SearchPhrase == null || (r.RentIssuer.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                           || r.RentIssuer.LastName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new ColumnSelector<Rent>()
                {
                    {nameof(Rent.CreationDate), r => r.CreationDate },
                    {nameof(Rent.PayDate), r => r.PayDate },
                    {nameof(Rent.Paid), r => r.Paid },
                    {nameof(Rent.RentIssuer.FirstName), r => r.RentIssuer.FirstName },
                    {nameof(Rent.RentIssuer.LastName), r => r.RentIssuer.LastName },
                };

                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }

            var rents = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var rentsDtos = _mapper.Map<List<RentDto>>(rents);

            var result = new PagedResult<RentDto>(rentsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public AccountDto GetAccountInfo()
        {
            var userId = (int)_userContextService.GetUserId;

            var account = _dbContext.Accounts.Include(a => a.Role).FirstOrDefault(a => a.Id == userId);

            if (account is null)
                throw new NotFoundException("Account not found");

            var accountDto = _mapper.Map<AccountDto>(account);

            return accountDto;
        }
    }
}
