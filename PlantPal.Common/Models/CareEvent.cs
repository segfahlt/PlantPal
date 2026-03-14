namespace PlantPal.Common.Models;

public class CareEvent
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Type { get; set; } = string.Empty;
	public int IntervalDays { get; set; }
	public string? Notes { get; set; }
	public DateTime? StartDateUtc { get; set; }
	public bool IsActive { get; set; } = true;
}
