using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        // Need to add dto
        [HttpPost("register")]
        public ActionResult CreateAccount()
        {
            return Ok();
        }

        [HttpPost("login")]
        // Need to add dto
        public ActionResult Login()
        {
            return Ok();
        }

        [HttpGet("user/{email}")]
        public ActionResult GetUserByEmail([FromRoute] string email)
        {
            return NoContent();
        }
        [HttpGet("user/{email}/rents")]
        public ActionResult GetUserRentsByEmail([FromRoute]string email)
        {
            return NoContent();
        }
        /**
         * Probably doesn't work temporary
         */
        [HttpDelete("user/{id}")]
        [Authorize]
        public ActionResult DeleteUserById([FromRoute]int id)
        {
            return Ok();
        }
    }
}
