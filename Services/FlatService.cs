using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using FlatsAPI.Models;
using FlatsAPI.Settings.Permissions;
using FlatsAPI.Settings.Roles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IFlatService
    {
        int Create(CreateFlatDto dto);
        PagedResult<FlatDto> GetAll(SearchQuery query);
        FlatDto GetById(int id);
        PagedResult<RentDto> GetRentsById(SearchQuery query, int id);
        void ApplyTenantByIds(int flatId, int tenantId);
        void Delete(int id);
    }
    public class FlatService : IFlatService
    {
        private readonly FlatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IPermissionContext _permissionContext;

        public FlatService(
            FlatsDbContext dbContext,
            IMapper mapper,
            IUserContextService userContextService,
            IPermissionContext permissionContext
            ) {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _permissionContext = permissionContext;
        }
        public void ApplyTenantByIds(int flatId, int tenantId)
        {
            var flat = _dbContext.Flats.FirstOrDefault(f => f.Id == flatId);

            if (flat is null)
                throw new NotFoundException("Flat not found");

            var potentialTenant = _dbContext.Accounts.FirstOrDefault(a => a.Id == tenantId);

            if (potentialTenant is null)
                throw new NotFoundException("Account not found");

            var userId = (int)_userContextService.GetUserId;

            var isAllowedToAddTenantWithoutOwnership = _permissionContext.IsPermittedToPerformAction(FlatPermissions.ApplyTenantOthers, userId);

            if (flat.OwnerId != userId && !isAllowedToAddTenantWithoutOwnership)
                throw new ForbiddenException("You are not permitted to perform this action");

            flat.Tenants.Add(potentialTenant);
            _dbContext.Flats.Update(flat);
            _dbContext.SaveChanges();
        }

        public int Create(CreateFlatDto dto)
        {
            var flat = _mapper.Map<Flat>(dto);

            var blockOfFlats = _dbContext.BlockOfFlats.FirstOrDefault(b => b.Id == dto.BlockOfFlatsId);

            if (blockOfFlats is null)
                throw new NotFoundException("Block of flats not found");

            var userId = (int)_userContextService.GetUserId;

            var isAllowedToAddNewFlatToSpecifiedBlock = _permissionContext.IsPermittedToPerformAction(BlockOfFlatsPermissions.UpdateOthers, userId);

            if (blockOfFlats.OwnerId != userId && !isAllowedToAddNewFlatToSpecifiedBlock)
                throw new ForbiddenException("You are not permitted to perform this action");

            var isObligatedToCreateAnonymously = _permissionContext.IsPermittedToPerformAction(FlatPermissions.CreateAnonymously, userId);

            if (isObligatedToCreateAnonymously)
                flat.Owner = _dbContext.Accounts.FirstOrDefault(a => a.Id == dto.OwnerId) ?? null;
            else
                flat.Owner = _dbContext.Accounts.FirstOrDefault(a => a.Id == userId);

            flat.PricePerMeterSquaredWhenRented = dto.PricePerMeterSquaredWhenRented ?? null;

            _dbContext.Flats.Add(flat);
            _dbContext.SaveChanges();

            return flat.Id;
        }

        public void Delete(int id)
        {
            var flat = _dbContext.Flats.FirstOrDefault(f => f.Id == id);

            if (flat is null)
                throw new NotFoundException("Flat not found");

            var userId = (int)_userContextService.GetUserId;

            var isAllowedToDeleteOthersFlats = _permissionContext.IsPermittedToPerformAction(FlatPermissions.DeleteOthers, userId);

            if (flat.OwnerId != userId && !isAllowedToDeleteOthersFlats)
                throw new ForbiddenException("You are not permitted to perform this action");

            _dbContext.Flats.Remove(flat);
            _dbContext.SaveChanges();
        }

        public PagedResult<FlatDto> GetAll(SearchQuery query)
        {
            var baseQuery = _dbContext
                .Flats
                .Include(f => f.BlockOfFlats)
                .Include(f => f.Owner)
                .Where(f => query.SearchPhrase == null || (f.BlockOfFlats.Address.ToLower().Contains(query.SearchPhrase.ToLower())
                                                           || f.Owner.FirstName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()))
                                                           || f.Owner.LastName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new ColumnSelector<Flat>()
                {
                    { nameof(Flat.Number), f => f.Number },
                    { nameof(Flat.Floor), f => f.Floor },
                    { nameof(Flat.Area), f => f.Area },
                    { "ownerFirstName", f => f.Owner.FirstName },
                    { "ownerLastName", f => f.Owner.LastName },
                    { "blockOfFlatsAddress", f => f.BlockOfFlats.Address },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }

            var flats = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var flatsDtos = _mapper.Map<List<FlatDto>>(flats);

            var result = new PagedResult<FlatDto>(flatsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public FlatDto GetById(int id)
        {
            var flat = _dbContext.Flats.FirstOrDefault(f => f.Id == id);

            if (flat is null)
                throw new NotFoundException("Flat not found");

            var result = _mapper.Map<FlatDto>(flat);
            return result;
        }

        public PagedResult<RentDto> GetRentsById(SearchQuery query, int id)
        {
            var baseQuery = _dbContext.Rents
                .Include(r => r.Owner)
                .Include(r => r.Flat)
                .Where(r => r.FlatId == id && (query.SearchPhrase == null || (r.Owner.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                           || r.Owner.LastName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()))
                                                               || r.Flat.BlockOfFlats.Address.ToLower()
                                                                    .Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new ColumnSelector<Rent>()
                {
                    { "flatNumber", r => r.Flat.Number },
                    { nameof(Rent.CreationDate), r => r.CreationDate },
                    { nameof(Rent.PayDate), r => r.PayDate },
                    { "ownerFirstName", r => r.Owner.FirstName },
                    { "ownerlastName", r => r.Owner.LastName },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

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
    }
}
