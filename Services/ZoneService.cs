using PlantPal.Abstraction;
using PlantPal.Models;

namespace PlantPal.Services;


public class ZoneService : IZoneService
{
	private readonly List<Zone> _zones = new();

	public List<Zone> GetAll() => _zones;

	public Zone? Get(Guid id) => _zones.FirstOrDefault(z => z.Id == id);

	public void Add(Zone zone)
	{
		_zones.Add(zone);
	}

	public void Update(Zone zone)
	{
		var existing = Get(zone.Id);
		if (existing is null) return;

		existing.Name = zone.Name;
		existing.Description = zone.Description;
	}

	public void Remove(Guid id)
	{
		var zone = Get(id);
		if (zone is not null)
		{
			_zones.Remove(zone);
		}
	}
}
