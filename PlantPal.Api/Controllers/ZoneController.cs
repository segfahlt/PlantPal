using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PlantPal.Abstraction;
using PlantPal.Common.Models;

namespace PlantPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZoneController : ControllerBase
{
	private readonly IZoneService _zoneService;

	public ZoneController(IZoneService zoneService)
	{
		_zoneService = zoneService;
	}

	[HttpGet]
	public ActionResult<List<Zone>> GetAll() => _zoneService.GetAll();

	[HttpGet("{id}")]
	public ActionResult<Zone> Get(Guid id)
	{
		var zone = _zoneService.Get(id);
		return zone is null ? NotFound() : Ok(zone);
	}

	[HttpPost]
	public IActionResult Create([FromBody] Zone zone)
	{
		_zoneService.Add(zone);
		return CreatedAtAction(nameof(Get), new { id = zone.Id }, zone);
	}

	[HttpPut("{id}")]
	public IActionResult Update(Guid id, [FromBody] Zone zone)
	{
		var existing = _zoneService.Get(id);
		if (existing is null) return NotFound();

		zone.Id = id;
		_zoneService.Update(zone);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public IActionResult Delete(Guid id)
	{
		var existing = _zoneService.Get(id);
		if (existing is null) return NotFound();

		_zoneService.Remove(id);
		return NoContent();
	}
}
