using PlantPal.Models;

namespace PlantPal.Abstraction;

public interface IPlantService
{
    List<Plant> GetAll();
    Plant? Get(Guid id);
    void Add(Plant plant);
    void Remove(Guid id);
}
