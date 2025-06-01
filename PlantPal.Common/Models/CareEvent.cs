namespace PlantPal.Common.Models;

public class CareEvent
{
    public string Type { get; set; } = string.Empty; // e.g., Water, Fertilize, Harvest
    public int IntervalDays { get; set; } // e.g., every 3 days
}
