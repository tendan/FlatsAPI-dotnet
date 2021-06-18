using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController : Controller
    {
        /**
         * Needs to be refactored
         */
        [HttpGet]
        public ActionResult GetInvoice()
        {
            return NoContent();
        }
    }
}
