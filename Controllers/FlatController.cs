using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Models;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/flats")]
    public class FlatController : Controller
    {
        // need to add dto
        [HttpPost]
        public ActionResult AddNewFlat([FromBody]CreateFlatDto createFlatDto)
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult GetAllFlats([FromQuery]SearchQuery query)
        {
            return NoContent();
        }

        [HttpGet("free")]
        public ActionResult GetFreeFlats([FromQuery]SearchQuery query)
        {
            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult GetSingleFlat([FromRoute] int id) {
            return NoContent();
        }

        [HttpGet("{id}/rents")]
        public ActionResult GetSingleFlatRents([FromQuery]SearchQuery query, [FromRoute] int id)
        {
            return NoContent();
        }

        [HttpPost("{flatId}/apply")]
        public ActionResult ApplyFlatForTenant([FromRoute]int flatId, [FromQuery]int tenantId, [FromQuery]OwnerShip chosen) {
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveFlat([FromRoute]int id) {
            return Ok();
        }
}
}
