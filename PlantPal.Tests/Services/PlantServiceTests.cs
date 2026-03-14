using FluentAssertions;

using Moq;

using PlantPal.Abstraction;
using PlantPal.Common.Models;
using PlantPal.Services;

using Xunit;

namespace PlantPal.Tests.Services;

public class PlantServiceTests
{
	[Fact]
	public void Constructor_ShouldLoadPlantsFromDataStore()
	{
		var expectedPlant = new Plant { Name = "Loaded Plant" };
		var mockStore = new Mock<IDataStore>();
		mockStore.Setup(store => store.LoadPlants()).ReturnsAsync([expectedPlant]);

		var service = new PlantService(mockStore.Object);

		service.GetAll().Should().ContainSingle();
		service.GetAll()[0].Name.Should().Be("Loaded Plant");
	}
}
