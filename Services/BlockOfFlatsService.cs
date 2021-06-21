using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IBlockOfFlatsService
    {
        void Create(CreateBlockOfFlatsDto dto);
        PagedResult<BlockOfFlatsDto> GetAll();
        BlockOfFlatsDto GetById(int id);
        void DeleteById(int id);
    }
    public class BlockOfFlatsService : IBlockOfFlatsService
    {
        public BlockOfFlatsService() { }
        public void Create(CreateBlockOfFlatsDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public PagedResult<BlockOfFlatsDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public BlockOfFlatsDto GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
