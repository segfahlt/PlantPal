using Microsoft.AspNetCore.Mvc;
using PlantPal.Models;

namespace PlantPal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IPlantService _plantService;
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IPlantService plantService, IScheduleService scheduleService)
    {
        _plantService = plantService;
        _scheduleService = scheduleService;
    }

    [HttpGet("today")]
    public ActionResult<List<TaskSchedule>> GetTasksDueToday()
    {
        var plants = _plantService.GetAll();
        var tasks = _scheduleService.GetTasksDueToday(plants);
        return Ok(tasks);
    }
}
