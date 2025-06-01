namespace PlantPal.Models;

public class Zone
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

	public List<Plant>? Plants { get; set; } = [];

	public List<Picture>? Pictures { get; set; } = [];


}