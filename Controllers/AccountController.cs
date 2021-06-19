using FlatsAPI.Models;
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
        [HttpPost("register")]
        public ActionResult CreateAccount([FromBody]CreateAccountDto createAccountDto)
        {
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto loginDto)
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
