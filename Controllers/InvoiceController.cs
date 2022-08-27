using FlatsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatsAPI.Controllers;

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
    [Authorize]
    public ActionResult GetInvoice([FromRoute] int id)
    {
        var invoice = _invoiceService.GetInvoiceForSpecifiedAccount(id);

        byte[] fileContents = invoice.FileContents;
        string contentType = invoice.ContentType;
        string fileName = invoice.FileName;

        return File(fileContents, contentType, fileName);
    }
}
