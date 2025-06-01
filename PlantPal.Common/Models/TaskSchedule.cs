namespace PlantPal.Common.Models;

public class TaskSchedule
{
    public string PlantName { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}
