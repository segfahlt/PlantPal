using FluentAssertions;

using Moq;

using PlantPal.Abstraction;
using PlantPal.Common.Models;
using PlantPal.Services;

using Xunit;

namespace PlantPal.Tests.Services;

public class ZoneServiceTests

{
	private readonly ZoneService _service;
	private readonly Mock<IDataStore> _mockStore;

	public ZoneServiceTests()
	{
		_mockStore = new Mock<IDataStore>();

		// Setup: return an empty list of zones when loading
		_mockStore.Setup(ds => ds.LoadZones()).Returns(Task.Run(() => new List<Zone>()));
		_service = new ZoneService(_mockStore.Object);
	}

	[Fact]
	public void Add_ShouldStoreZone()
	{
		// Arrange
		var zone = new Zone { Id = Guid.NewGuid(), Name = "Mango Zone" };

		// Act
		_service.Add(zone);

		// Assert
		var all = _service.GetAll();
		all.Should().ContainSingle(z => z.Id == zone.Id);
	}

	[Fact]
	public void Get_ShouldReturnCorrectZone()
	{
		// Arrange
		var zone = new Zone { Id = Guid.NewGuid(), Name = "North Yard" };
		_service.Add(zone);

		// Act
		var result = _service.Get(zone.Id);

		// Assert
		result.Should().NotBeNull();
		result!.Name.Should().Be("North Yard");
	}

	[Fact]
	public void Update_ShouldModifyExistingZone()
	{
		// Arrange
		var zone = new Zone { Id = Guid.NewGuid(), Name = "Old Name" };
		_service.Add(zone);

		var updated = new Zone { Id = zone.Id, Name = "New Name" };

		// Act
		_service.Update(updated);

		// Assert
		var result = _service.Get(zone.Id);
		result.Should().NotBeNull();
		result!.Name.Should().Be("New Name");
	}

	[Fact]
	public void Remove_ShouldDeleteZone()
	{
		// Arrange
		var zone = new Zone { Id = Guid.NewGuid(), Name = "To Be Deleted" };
		_service.Add(zone);

		// Act
		_service.Remove(zone.Id);

		// Assert
		_service.GetAll().Should().BeEmpty();
		_service.Get(zone.Id).Should().BeNull();
	}
}
