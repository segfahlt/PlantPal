using FluentAssertions;

using PlantPal.Common;
using PlantPal.Services;

using Xunit;

namespace PlantPal.Tests.Services;

public class JsonFileDataStoreTests : IDisposable
{
	private readonly string? _originalRepoUrl = Environment.GetEnvironmentVariable(Constants.DataRepoUrlEnvVariable);
	private readonly string? _originalRepoPath = Environment.GetEnvironmentVariable(Constants.RepoPathEnvVariable);
	private readonly string _workingRepoPath = Path.Combine(Path.GetTempPath(), "PlantPal.Tests", Guid.NewGuid().ToString("N"));

	[Fact]
	public async Task LoadPlantsAndZones_ShouldReadSchemaBasedRepository()
	{
		var sourceRepoPath = FindLocalDataRepoPath();
		Environment.SetEnvironmentVariable(Constants.DataRepoUrlEnvVariable, sourceRepoPath);
		Environment.SetEnvironmentVariable(Constants.RepoPathEnvVariable, _workingRepoPath);

		var store = new JsonFileDataStore(new DataRepoService());

		var zones = await store.LoadZones();
		var plants = await store.LoadPlants();

		zones.Should().ContainSingle();
		plants.Should().ContainSingle();

		var zone = zones.Single();
		var plant = plants.Single();

		zone.Name.Should().Be("Turmeric Grove");
		plant.Name.Should().Be("Marshmallow Patch");
		plant.ScientificName.Should().Be("Althaea officinalis");
		plant.ZoneId.Should().Be(zone.Id);
		plant.Zone.Should().NotBeNull();
		plant.Zone!.Name.Should().Be(zone.Name);
		plant.IsOutdoor.Should().BeTrue();
		plant.IsPerennial.Should().BeTrue();
		plant.CareEvents.Should().ContainSingle(care => care.Type == "water" && care.IntervalDays == 3);

		Directory.Exists(Path.Combine(_workingRepoPath, ".git")).Should().BeTrue();
	}

	public void Dispose()
	{
		Environment.SetEnvironmentVariable(Constants.DataRepoUrlEnvVariable, _originalRepoUrl);
		Environment.SetEnvironmentVariable(Constants.RepoPathEnvVariable, _originalRepoPath);

		if (!Directory.Exists(_workingRepoPath))
			return;

		for (var attempt = 0; attempt < 5; attempt++)
		{
			try
			{
				Directory.Delete(_workingRepoPath, true);
				return;
			}
			catch (UnauthorizedAccessException)
			{
				if (attempt == 4)
					return;

				Thread.Sleep(200);
			}
			catch (IOException)
			{
				if (attempt == 4)
					return;

				Thread.Sleep(200);
			}
		}
	}

	private static string FindLocalDataRepoPath()
	{
		var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

		while (currentDirectory is not null)
		{
			var candidate = Path.Combine(currentDirectory.FullName, "PlantPal-Data");
			if (Directory.Exists(Path.Combine(candidate, ".git")) && File.Exists(Path.Combine(candidate, "schema.md")))
				return candidate;

			currentDirectory = currentDirectory.Parent;
		}

		throw new DirectoryNotFoundException("Could not find the local PlantPal-Data repository for integration testing.");
	}
}
