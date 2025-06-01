using PlantPal.Common.Models;

namespace PlantPal.Abstraction;

public interface IScheduleService
{
    List<TaskSchedule> GetTasksDueToday(List<Plant> plants);
}
