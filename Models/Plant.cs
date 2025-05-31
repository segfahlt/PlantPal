namespace PlantPal.Models;

public class Plant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime PlantedDate { get; set; }
    public List<CareEvent> CareEvents { get; set; } = new();
}
