using PlantPal.Abstraction;
using PlantPal.Common.Models;

namespace PlantPal.Services;

public class ScheduleService : IScheduleService
{
    public List<TaskSchedule> GetTasksDueToday(List<Plant> plants)
    {
        var tasks = new List<TaskSchedule>();
        var today = DateTime.Today;

        foreach (var plant in plants)
        {
            foreach (var care in plant.CareEvents)
            {
                var daysSincePlanted = (today - plant.PlantedDate).Days;
                if (daysSincePlanted >= 0 && daysSincePlanted % care.IntervalDays == 0)
                {
                    tasks.Add(new TaskSchedule
                    {
                        PlantName = plant.Name,
                        Task = care.Type,
                        DueDate = today
                    });
                }
            }
        }

        return tasks;
    }
}
