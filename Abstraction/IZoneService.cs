namespace PlantPal.Abstraction;

using PlantPal.Models;

public interface IZoneService
{
	List<Zone> GetAll();
	Zone? Get(Guid id);
	void Add(Zone zone);
	void Update(Zone zone);
	void Remove(Guid id);
}
