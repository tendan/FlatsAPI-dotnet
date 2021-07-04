using FlatsAPI.Authorization;
using FlatsAPI.Models;
using FlatsAPI.Services;
using FlatsAPI.Settings.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/blocks")]
    public class BlockOfFlatsController : Controller
    {
        private readonly IBlockOfFlatsService _blockOfFlatsService;

        public BlockOfFlatsController(IBlockOfFlatsService blockOfFlatsService)
        {
            _blockOfFlatsService = blockOfFlatsService;
        }
        [HttpPost]
        [PermissionAuthorize(BlockOfFlatsPermissions.Create)]
        public ActionResult CreateNewBlock([FromBody]CreateBlockOfFlatsDto createBlockOfFlatsDto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var blockOfFlatsId = _blockOfFlatsService.Create(createBlockOfFlatsDto);

            return Created($"/api/blocks/{userId}", null);
        }
        [HttpGet]
        public ActionResult<List<BlockOfFlatsDto>> GetAllBlocks([FromQuery]SearchQuery query)
        {
            return Ok(_blockOfFlatsService.GetAll(query));
        }
        [HttpGet("{id}")]
        public ActionResult<BlockOfFlatsDto> GetSpecifiedBlock([FromRoute]int id)
        {
            return Ok(_blockOfFlatsService.GetById(id));
        }
        [HttpGet("{id}/flats")]
        public ActionResult<List<FlatInBlockOfFlatsDto>> GetAllFlatsFromSpecifiedBlock([FromQuery]SearchQuery query, [FromRoute]int id)
        {
            return Ok(_blockOfFlatsService.GetAllFlatsById(query, id));
        }
        [HttpDelete("{id}")]
        [PermissionAuthorize(BlockOfFlatsPermissions.Delete)]
        public ActionResult DeleteBlock([FromRoute]int id)
        {
            _blockOfFlatsService.DeleteById(id);

            return NoContent();
        }
    }
}
