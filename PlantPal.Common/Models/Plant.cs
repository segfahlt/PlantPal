namespace PlantPal.Common.Models;

public class Plant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime PlantedDate { get; set; }
    public List<CareEvent> CareEvents { get; set; } = new();
    public Zone? Zone { get; set; }

    public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string? ImageUrl { get; set; }
	public string? Description { get; set; }
	public string? Notes { get; set; }
	public bool IsIndoor { get; set; } = false;
	public bool IsOutdoor { get; set; } = false;
	public bool IsPerennial { get; set; } = false;
	public bool IsAnnual { get; set; } = false;
	public List<Picture>? Pictures { get; set; } = new();


}
