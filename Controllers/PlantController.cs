using Microsoft.AspNetCore.Mvc;
using PlantPal.Models;

namespace PlantPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlantController : ControllerBase
{
    private readonly IPlantService _plantService;

    public PlantController(IPlantService plantService)
    {
        _plantService = plantService;
    }

    [HttpGet]
    public ActionResult<List<Plant>> GetAll() => _plantService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Plant> Get(Guid id)
    {
        var plant = _plantService.Get(id);
        return plant is null ? NotFound() : Ok(plant);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Plant plant)
    {
        _plantService.Add(plant);
        return CreatedAtAction(nameof(Get), new { id = plant.Id }, plant);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _plantService.Remove(id);
        return NoContent();
    }
}
