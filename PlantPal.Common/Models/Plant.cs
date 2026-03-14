namespace PlantPal.Common.Models;

public class Plant
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = string.Empty;
	public string? CommonName { get; set; }
	public string? ScientificName { get; set; }
	public Guid? ZoneId { get; set; }
	public Zone? Zone { get; set; }
	public DateTime? PlantedDate { get; set; }
	public string? Status { get; set; }
	public string? LifeCycle { get; set; }
	public string? Environment { get; set; }
	public double? GpsAccuracyMeters { get; set; }
	public Guid? PrimaryPictureId { get; set; }
	public List<string> Tags { get; set; } = [];
	public List<CareEvent> CareEvents { get; set; } = [];
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime? DeletedAtUtc { get; set; }
	public int Version { get; set; } = 1;
	public string? SourceDeviceId { get; set; }

	public double? Latitude { get; set; }
	public double? Longitude { get; set; }
	public string? ImageUrl { get; set; }
	public string? Description { get; set; }
	public string? Notes { get; set; }
	public bool IsIndoor
	{
		get => string.Equals(Environment, "indoor", StringComparison.OrdinalIgnoreCase);
		set
		{
			if (value)
				Environment = "indoor";
		}
	}
	public bool IsOutdoor
	{
		get => string.Equals(Environment, "outdoor", StringComparison.OrdinalIgnoreCase);
		set
		{
			if (value)
				Environment = "outdoor";
		}
	}
	public bool IsPerennial
	{
		get => string.Equals(LifeCycle, "perennial", StringComparison.OrdinalIgnoreCase);
		set
		{
			if (value)
				LifeCycle = "perennial";
		}
	}
	public bool IsAnnual
	{
		get => string.Equals(LifeCycle, "annual", StringComparison.OrdinalIgnoreCase);
		set
		{
			if (value)
				LifeCycle = "annual";
		}
	}
	public List<Picture>? Pictures { get; set; } = new();


}
