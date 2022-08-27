using FlatsAPI.Authorization;
using FlatsAPI.Models;
using FlatsAPI.Services;
using FlatsAPI.Settings.Permissions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace FlatsAPI.Controllers;

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
    [PermissionAuthorize(FlatPermissions.Create)]
    public ActionResult AddNewFlat([FromBody] CreateFlatDto createFlatDto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var flatId = _flatService.Create(createFlatDto);

        return Created($"/api/flats/{flatId}", null);
    }

    [HttpGet]
    public ActionResult<List<FlatDto>> GetAllFlats([FromQuery] SearchQuery query)
    {
        return Ok(_flatService.GetAll(query));
    }

    [HttpGet("free")]
    public ActionResult GetFreeFlats([FromQuery] SearchQuery query)
    {
        return NoContent();
    }

    [HttpGet("{id}")]
    public ActionResult<FlatDto> GetSingleFlat([FromRoute] int id)
    {
        return Ok(_flatService.GetById(id));
    }

    [HttpGet("{id}/rents")]
    public ActionResult<List<RentDto>> GetSingleFlatRents([FromQuery] SearchQuery query, [FromRoute] int id)
    {
        return Ok(_flatService.GetRentsById(query, id));
    }

    [HttpPost("{flatId}/apply")]
    [PermissionAuthorize(FlatPermissions.ApplyTenant)]
    public ActionResult ApplyFlatForTenant([FromRoute] int flatId, [FromQuery] int tenantId)
    {
        _flatService.ApplyTenantByIds(flatId, tenantId);
        return Created($"/api/flats/{flatId}", null);
    }

    [HttpDelete("{id}")]
    [PermissionAuthorize(FlatPermissions.Delete)]
    public ActionResult RemoveFlat([FromRoute] int id)
    {
        _flatService.Delete(id);

        return NoContent();
    }
}
