using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Services
{
    public interface IBlockOfFlatsService
    {
        void CreateBlockOfFlats(CreateBlockOfFlatsDto dto);
        ICollection<BlockOfFlatsDto> GetAllBlocksOfFlats();
        BlockOfFlatsDto GetSpecifiedBlockById(int id);
        void DeleteBlockById(int id);
    }
    public class BlockOfFlatsService : IBlockOfFlatsService
    {
        public BlockOfFlatsService() { }
        public void CreateBlockOfFlats(CreateBlockOfFlatsDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteBlockById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<BlockOfFlatsDto> GetAllBlocksOfFlats()
        {
            throw new NotImplementedException();
        }

        public BlockOfFlatsDto GetSpecifiedBlockById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
