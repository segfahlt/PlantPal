namespace PlantPal.Services;

using System.Text;
using System.Text.Json;

using PlantPal.Abstraction;
using PlantPal.Common;
using PlantPal.Common.Models;

public class JsonFileDataStore : IDataStore
{
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

	public async Task<List<Zone>> LoadZones()
	{
		await _dataRepoService.SyncFromRepo();
		return LoadZonesFromDisk();
	}

	public async Task<bool> SaveZones(List<Zone> zones)
	{
		try
		{
			await _dataRepoService.SyncFromRepo();
			var zonesPath = Constants.GetZonesPath();
			Directory.CreateDirectory(zonesPath);

			foreach (var zone in zones)
			{
				var zoneDirectory = Path.Combine(zonesPath, CreateSlug(zone.Name));
				Directory.CreateDirectory(zoneDirectory);

				var path = Path.Combine(zoneDirectory, "zone.json");
				var json = JsonSerializer.Serialize(ToZoneRecord(zone), _jsonOptions);
				await File.WriteAllTextAsync(path, json, Encoding.UTF8);
			}

			return await _dataRepoService.SyncToRepo();
		}
		catch
		{
			return false;
		}
	}

	public async Task<List<Plant>> LoadPlants()
	{
		await _dataRepoService.SyncFromRepo();

		var zones = LoadZonesFromDisk();
		var zoneLookup = zones.ToDictionary(zone => zone.Id);
		var plants = LoadRecords<Plant>(Constants.GetPlantsPath(), "plant.json");

		foreach (var plant in plants)
		{
			plant.Zone = null;

			if (plant.ZoneId is Guid zoneId && zoneLookup.TryGetValue(zoneId, out var zone))
				plant.Zone = zone;
		}

		return plants;
	}

	public async Task<bool> SavePlants(List<Plant> plants)
	{
		try
		{
			await _dataRepoService.SyncFromRepo();
			var plantsPath = Constants.GetPlantsPath();
			Directory.CreateDirectory(plantsPath);

			foreach (var plant in plants)
			{
				var speciesDirectory = Path.Combine(plantsPath, CreateSlug(plant.ScientificName ?? plant.Name));
				var plantDirectory = Path.Combine(speciesDirectory, CreatePlantSlug(plant));
				Directory.CreateDirectory(plantDirectory);

				var path = Path.Combine(plantDirectory, "plant.json");
				var json = JsonSerializer.Serialize(ToPlantRecord(plant), _jsonOptions);
				await File.WriteAllTextAsync(path, json, Encoding.UTF8);
			}

			return await _dataRepoService.SyncToRepo();
		}
		catch
		{
			return false;
		}
	}

	private List<Zone> LoadZonesFromDisk() =>
		LoadRecords<Zone>(Constants.GetZonesPath(), "zone.json");

	private List<T> LoadRecords<T>(string rootPath, string fileName) where T : class
	{
		if (!Directory.Exists(rootPath))
			return [];

		return Directory
			.EnumerateFiles(rootPath, fileName, SearchOption.AllDirectories)
			.Select(ReadRecord<T>)
			.Where(record => record is not null)
			.Cast<T>()
			.Where(IsActiveRecord)
			.ToList();
	}

	private T? ReadRecord<T>(string path) where T : class
	{
		var json = File.ReadAllText(path);
		return JsonSerializer.Deserialize<T>(json, _jsonOptions);
	}

	private static bool IsActiveRecord<T>(T record) where T : class =>
		record switch
		{
			Zone zone => zone.DeletedAtUtc is null,
			Plant plant => plant.DeletedAtUtc is null,
			_ => true
		};

	private static string CreatePlantSlug(Plant plant)
	{
		var shortId = plant.Id.ToString("N")[..8];
		return $"{CreateSlug(plant.Name)}--{shortId}";
	}

	private static object ToZoneRecord(Zone zone) => new
	{
		zone.Id,
		zone.Name,
		zone.Description,
		zone.ParentZoneId,
		zone.Latitude,
		zone.Longitude,
		zone.GeoRadiusMeters,
		zone.SunExposure,
		zone.SoilNotes,
		zone.Tags,
		zone.CreatedAtUtc,
		zone.UpdatedAtUtc,
		zone.DeletedAtUtc,
		zone.Version,
		zone.SourceDeviceId
	};

	private static object ToPlantRecord(Plant plant) => new
	{
		plant.Id,
		plant.Name,
		plant.CommonName,
		plant.ScientificName,
		plant.ZoneId,
		plant.Description,
		plant.Notes,
		plant.PlantedDate,
		plant.Status,
		plant.LifeCycle,
		plant.Environment,
		plant.Latitude,
		plant.Longitude,
		plant.GpsAccuracyMeters,
		plant.PrimaryPictureId,
		plant.Tags,
		CareEvents = plant.CareEvents.Select(careEvent => new
		{
			careEvent.Id,
			careEvent.Type,
			careEvent.IntervalDays,
			careEvent.Notes,
			careEvent.StartDateUtc,
			careEvent.IsActive
		}),
		plant.CreatedAtUtc,
		plant.UpdatedAtUtc,
		plant.DeletedAtUtc,
		plant.Version,
		plant.SourceDeviceId
	};

	private static string CreateSlug(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return "unknown";

		var builder = new StringBuilder(value.Length);
		var previousWasDash = false;

		foreach (var character in value.Trim().ToLowerInvariant())
		{
			if (char.IsLetterOrDigit(character))
			{
				builder.Append(character);
				previousWasDash = false;
				continue;
			}

			if (previousWasDash)
				continue;

			builder.Append('-');
			previousWasDash = true;
		}

		return builder.ToString().Trim('-');
	}
}
