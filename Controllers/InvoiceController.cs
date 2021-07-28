using FlatsAPI.Services;
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
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        [HttpGet("{id}")]
        public ActionResult GetInvoice([FromRoute]int id)
        {
            var invoice = _invoiceService.GetInvoiceForSpecifiedAccount(id);

            byte[] fileContents = invoice.FileContents;
            string contentType = invoice.ContentType;
            string fileName = invoice.FileName;

            return base.File(fileContents, contentType, fileName);
        }
    }
}
