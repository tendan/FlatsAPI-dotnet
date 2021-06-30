using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Models;
using FlatsAPI.Services;
using System.Security.Claims;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/flats")]
    public class FlatController : Controller
    {
        private readonly IFlatService _flatService;

        public FlatController(IFlatService flatService)
        {
            _flatService = flatService;
        }
        // need to add dto
        [HttpPost]
        public ActionResult AddNewFlat([FromBody]CreateFlatDto createFlatDto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var blockOfFlatsId = _flatService.Create(createFlatDto);

            return Created($"/api/blocks/{userId}", null);
        }

        [HttpGet]
        public ActionResult<List<FlatDto>> GetAllFlats([FromQuery]SearchQuery query)
        {
            return Ok(_flatService.GetAll(query));
        }

        [HttpGet("free")]
        public ActionResult GetFreeFlats([FromQuery]SearchQuery query)
        {
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<FlatDto> GetSingleFlat([FromRoute] int id) 
        {
            return Ok(_flatService.GetById(id));
        }

        [HttpGet("{id}/rents")]
        public ActionResult<List<RentDto>> GetSingleFlatRents([FromQuery]SearchQuery query, [FromRoute] int id)
        {
            return Ok(_flatService.GetRentsById(query, id));
        }

        [HttpPost("{flatId}/apply")]
        public ActionResult ApplyFlatForTenant([FromRoute]int flatId, [FromQuery]int tenantId, [FromQuery]OwnerShip chosen) 
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveFlat([FromRoute]int id) 
        {
            _flatService.Delete(id);

            return NoContent();
        }
}
}
