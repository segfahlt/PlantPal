using PlantPal.Models;

public interface IScheduleService
{
    List<TaskSchedule> GetTasksDueToday(List<Plant> plants);
}
