using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    public class TenantController : Controller
    {
        [HttpPost]
        public ActionResult CreateNewTenant()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult GetSingleTenantById([FromRoute]int id)
        {
            return NoContent();
        }

        [HttpGet("{id}/rents")]
        public ActionResult GetSingleTenantRentsById([FromRoute]int id)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTenantById([FromRoute]int id)
        {
            return Ok();
        }
    }
}
