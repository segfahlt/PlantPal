namespace PlantPal.Common.Models;

public class Zone
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public Guid? ParentZoneId { get; set; }
	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
	public double? GeoRadiusMeters { get; set; }
	public string? SunExposure { get; set; }
	public string? SoilNotes { get; set; }
	public List<string> Tags { get; set; } = [];
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime? DeletedAtUtc { get; set; }
	public int Version { get; set; } = 1;
	public string? SourceDeviceId { get; set; }

	public List<Plant>? Plants { get; set; } = [];

	public List<Picture>? Pictures { get; set; } = [];


}
