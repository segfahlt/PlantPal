namespace PlantPal.Services;

using System.Reflection.Metadata;
using System.Text.Json;

using PlantPal.Abstraction;
using PlantPal.Models;

public class JsonFileDataStore : IDataStore
{
	private readonly string _dataPath;
	private readonly IDataRepoService _dataRepoService;
	private readonly JsonSerializerOptions _jsonOptions = new()
	{
		WriteIndented = true,
		PropertyNameCaseInsensitive = true
	};

	public JsonFileDataStore(IDataRepoService dataRepoService)
	{
		_dataRepoService = dataRepoService;
	}

	public async Task<List<Zone>> LoadZones() => await LoadData<List<Zone>>();
	public async Task<bool> SaveZones(List<Zone> zones) => await SaveData(zones);

	public async Task<List<Plant>> LoadPlants() => await LoadData<List<Plant>>();
	public async Task<bool> SavePlants(List<Plant> plants) => await SaveData(plants);

	private async Task<T> LoadData<T>()
	{
		var typeName = string.Empty;
		var type = typeof(T);

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			var itemType = type.GetGenericArguments()[0];
			typeName = itemType.Name;
		}

		var fileName = Path.Combine(Constants.DataPath, typeName + ".json");
		await _dataRepoService.SyncFromRepo();
		var path = Path.Combine(_dataPath, fileName);
		if (!File.Exists(path)) return Activator.CreateInstance<T>();
		var json = File.ReadAllText(path);

		//TODO: read images from images directory
		return JsonSerializer.Deserialize<T>(json, _jsonOptions) ?? Activator.CreateInstance<T>();
	}

	private async Task<bool> SaveData<T>(T data)
	{
		try
		{
			var typeName = string.Empty;
			var type = typeof(T);

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				var itemType = type.GetGenericArguments()[0];
				typeName = itemType.Name;
			}

			var fileName = Path.Combine(Constants.DataPath, typeName + ".json");

			await _dataRepoService.SyncFromRepo();
			var path = Path.Combine(_dataPath, fileName);
			var json = JsonSerializer.Serialize(data, _jsonOptions);
			File.WriteAllText(path, json);
			await _dataRepoService.SyncToRepo();
			return true;
		}
		catch (Exception ex)
		{
			//TODO: Log it.
			return false;
		}

	}
}
