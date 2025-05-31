using PlantPal.Models;


namespace PlantPal.Abstraction;

public interface IScheduleService
{
    List<TaskSchedule> GetTasksDueToday(List<Plant> plants);
}
