using PlantPal.Abstraction;
using PlantPal.Common.Models;

namespace PlantPal.Services;

public class ScheduleService : IScheduleService
{
	public List<TaskSchedule> GetTasksDueToday(List<Plant> plants)
	{
		var tasks = new List<TaskSchedule>();
		var today = DateTime.UtcNow.Date;

		foreach (var plant in plants)
		{
			foreach (var care in plant.CareEvents.Where(care => care.IsActive && care.IntervalDays > 0))
			{
				var anchorDate = care.StartDateUtc?.Date ?? plant.PlantedDate?.Date;
				if (anchorDate is null)
					continue;

				var daysSinceAnchor = (today - anchorDate.Value).Days;
				if (daysSinceAnchor < 0 || daysSinceAnchor % care.IntervalDays != 0)
					continue;

				tasks.Add(new TaskSchedule
				{
					PlantName = plant.Name,
					Task = care.Type,
					DueDate = today
				});
			}
		}

		return tasks;
	}
}
