namespace PlantPal.Abstraction;

using PlantPal.Models;

public interface IDataStore
{
	Task<List<Zone>> LoadZones();
	Task<bool> SaveZones(List<Zone> zones);

	Task<List<Plant>> LoadPlants();
	Task<bool> SavePlants(List<Plant> plants);

}
