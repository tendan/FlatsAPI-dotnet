using FlatsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/blocks")]
    public class BlockOfFlatsController : Controller
    {
        [HttpPost]
        // Need to add dto
        public ActionResult CreateNewBlock([FromBody]CreateBlockOfFlatsDto createBlockOfFlatsDto)
        {
            return Ok();
        }
        [HttpGet]
        public ActionResult GetAllBlocks()
        {
            return NoContent();
        }
        [HttpGet("{id}")]
        public ActionResult GetSpecifiedBlock([FromRoute]int id)
        {
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteBlock([FromRoute]int id)
        {
            return Ok();
        }
    }
}
