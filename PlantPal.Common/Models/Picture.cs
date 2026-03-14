namespace PlantPal.Common.Models;

public class Picture
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime TakenAtUtc { get; set; } = DateTime.UtcNow;
	public string FileName { get; set; } = string.Empty;
	public string? RelativePath { get; set; }
	public string? Caption { get; set; }
	public string? MimeType { get; set; }
	public string? Base64Data { get; set; }
	public Guid? ZoneId { get; set; }
	public Guid? PlantId { get; set; }
	public Guid? ObservationId { get; set; }
	public string? Sha1Hash { get; set; }
	public int? Width { get; set; }
	public int? Height { get; set; }
	public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
	public DateTime? DeletedAtUtc { get; set; }
	public int Version { get; set; } = 1;
	public string? SourceDeviceId { get; set; }

}
