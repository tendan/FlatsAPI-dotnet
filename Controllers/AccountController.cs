using FlatsAPI.Models;
using FlatsAPI.Services;
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
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public ActionResult CreateAccount([FromBody] CreateAccountDto createAccountDto)
        {
            var account = _accountService.Create(createAccountDto);

            return Created($"/api/user/{createAccountDto.Email}", null);
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            return Ok(_accountService.GenerateJwt(loginDto));
        }
        [HttpGet("user")]
        [Authorize]
        public ActionResult GetAccountInfo()
        {
            return Ok(_accountService.GetAccountInfo());
        }

        [HttpGet("user/{email}")]
        public ActionResult GetAccountByEmail([FromRoute] string email)
        {
            return Ok(_accountService.GetByEmail(email));
        }
        [HttpGet("user/{email}/rents")]
        public ActionResult GetAccountRentsByEmail([FromQuery]SearchQuery query, [FromRoute]string email)
        {
            return Ok(_accountService.GetRentsByEmail(query, email));
        }
        [HttpPatch("user/{id}")]
        [Authorize]
        public ActionResult UpdateAccountInfo([FromRoute]int id, [FromBody]UpdateAccountDto dto)
        {
            _accountService.Update(id, dto);

            return NoContent();
        }
        [HttpDelete("user/{id}")]
        [Authorize]
        public ActionResult DeleteUserById([FromRoute]int id)
        {
            _accountService.DeleteById(id);

            return NoContent();
        }
    }
}
