namespace PlantPal.Common.Models;

public class Picture
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime TakenAt { get; set; } = DateTime.UtcNow;
	public string FileName { get; set; } = string.Empty;      // e.g., "zone1_2025-05-31.jpg"
	public string? Caption { get; set; }
	public string? Base64Data { get; set; }                   // optional, only if stored inline
	public Guid? ZoneId { get; set; } = null; // optional, if picture is associated with a zone
	public Guid? PlantId { get; set; } = null; // optional, if picture is associated with a plant
	public string SHA1Hath { get; set; } = string.Empty; // SHA1 hash of the file content for integrity check

}
