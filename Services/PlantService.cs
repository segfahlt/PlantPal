using PlantPal.Models;
using PlantPal.Abstraction;

namespace PlantPal.Services;

public class PlantService : IPlantService
{
    private readonly List<Plant> _plants = new();

    public List<Plant> GetAll() => _plants;

    public Plant? Get(Guid id) => _plants.FirstOrDefault(p => p.Id == id);

    public void Add(Plant plant) => _plants.Add(plant);

    public void Remove(Guid id)
    {
        var plant = Get(id);
        if (plant != null)
            _plants.Remove(plant);
    }
}
