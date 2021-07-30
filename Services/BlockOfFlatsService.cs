using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;
using FlatsAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FlatsAPI.Settings.Roles;
using FlatsAPI.Settings.Permissions;

namespace FlatsAPI.Services
{
    public interface IBlockOfFlatsService
    {
        int Create(CreateBlockOfFlatsDto dto);
        PagedResult<BlockOfFlatsDto> GetAll(SearchQuery query);
        BlockOfFlatsDto GetById(int id);
        PagedResult<FlatInBlockOfFlatsDto> GetAllFlatsById(SearchQuery query, int id);
        void DeleteById(int id);
    }
    public class BlockOfFlatsService : IBlockOfFlatsService
    {
        private readonly FlatsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IPermissionContext _permissionContext;

        public BlockOfFlatsService(
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
        public int Create(CreateBlockOfFlatsDto dto)
        {
            var blockOfFlats = _mapper.Map<BlockOfFlats>(dto);

            var userId = (int)_userContextService.GetUserId;

            var isObligatedToCreateAnonymously = _permissionContext.IsPermittedToPerformAction(BlockOfFlatsPermissions.CreateAnonymously, userId);

            if (isObligatedToCreateAnonymously)
                blockOfFlats.Owner = _dbContext.Accounts.FirstOrDefault(a => a.Id == dto.OwnerId) ?? null;
            else
                blockOfFlats.Owner = _dbContext.Accounts.FirstOrDefault(a => a.Id == userId);

            _dbContext.BlockOfFlats.Add(blockOfFlats);
            _dbContext.SaveChanges();

            return blockOfFlats.Id;
        }

        public void DeleteById(int id)
        {
            var blockOfFlats = _dbContext
                .BlockOfFlats
                .FirstOrDefault(b => b.Id == id);

            if (blockOfFlats is null)
                throw new NotFoundException("Block of flats not found");

            _userContextService.AuthorizeAccess(blockOfFlats.OwnerId, BlockOfFlatsPermissions.DeleteOthers);

            _dbContext.BlockOfFlats.Remove(blockOfFlats);
            _dbContext.SaveChanges();
        }

        public PagedResult<BlockOfFlatsDto> GetAll(SearchQuery query)
        {
            var baseQuery = _dbContext
                .BlockOfFlats
                .Include(b => b.Owner)
                .Where(b => query.SearchPhrase == null || (b.Address.ToLower().Contains(query.SearchPhrase.ToLower())
                                                           || b.Owner.FirstName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()))
                                                           || b.Owner.LastName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new ColumnSelector<BlockOfFlats>()
                {
                    { nameof(BlockOfFlats.Address), b => b.Address },
                    { nameof(BlockOfFlats.Floors), b => b.Floors },
                    { nameof(BlockOfFlats.Margin), b => b.Margin },
                    { "ownerFirstName", b => b.Owner.FirstName },
                    { "ownerLastName", b => b.Owner.LastName },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }

            var blocks = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var blocksDtos = _mapper.Map<List<BlockOfFlatsDto>>(blocks);

            var result = new PagedResult<BlockOfFlatsDto>(blocksDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public BlockOfFlatsDto GetById(int id)
        {
            var blockOfFlats = _dbContext
                .BlockOfFlats
                .FirstOrDefault(b => b.Id == id);

            if (blockOfFlats is null)
                throw new NotFoundException("Block of flats not found");

            var result = _mapper.Map<BlockOfFlatsDto>(blockOfFlats);
            return result;
        }

        public PagedResult<FlatInBlockOfFlatsDto> GetAllFlatsById(SearchQuery query, int id)
        {
            var baseQuery = _dbContext
                .Flats
                .Include(f => f.BlockOfFlats)
                .Include(f => f.Owner)
                .Where(f => f.BlockOfFlatsId == id && (query.SearchPhrase == null || (f.Owner.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                           || f.Owner.LastName.ToLower()
                                                               .Contains(query.SearchPhrase.ToLower()))
                                                               || f.BlockOfFlats.Address.ToLower()
                                                                    .Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new ColumnSelector<Flat>()
                {
                    { nameof(Flat.Number), b => b.Number },
                    { nameof(Flat.Floor), b => b.Floor },
                    { nameof(Flat.Area), b => b.Area },
                    { "ownerFirstName", b => b.Owner.FirstName },
                    { "ownerLastName", b => b.Owner.LastName },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn) : baseQuery.OrderByDescending(selectedColumn);
            }

            var flats = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var flatsDtos = _mapper.Map<List<FlatInBlockOfFlatsDto>>(flats);

            var result = new PagedResult<FlatInBlockOfFlatsDto>(flatsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }
    }
}
