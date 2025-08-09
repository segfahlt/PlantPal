using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using PlantPal.Abstraction;
using PlantPal.Common.Models;
using PlantPal.Controllers;

using Xunit;

namespace PlantPal.Tests.Controller;

public class ZoneControllerTests
{
        private readonly Mock<IZoneService> _mockService;
        private readonly ZoneController _controller;

        public ZoneControllerTests()
        {
                _mockService = new Mock<IZoneService>();
                _controller = new ZoneController(_mockService.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllZones()
        {
                // Arrange
                var zones = new List<Zone> { new() { Id = Guid.NewGuid(), Name = "Kitchen" } };
                _mockService.Setup(s => s.GetAll()).Returns(zones);

                // Act
                var result = _controller.GetAll();

                // Assert
                result.Value.Should().BeEquivalentTo(zones);
        }

        [Fact]
        public void Get_ShouldReturnZone_WhenFound()
        {
                // Arrange
                var id = Guid.NewGuid();
                var zone = new Zone { Id = id, Name = "Living Room" };
                _mockService.Setup(s => s.Get(id)).Returns(zone);

                // Act
                var result = _controller.Get(id);

                // Assert
                var ok = result.Result as OkObjectResult;
                ok.Should().NotBeNull();
                ok!.Value.Should().BeEquivalentTo(zone);
        }

        [Fact]
        public void Get_ShouldReturnNotFound_WhenMissing()
        {
                // Arrange
                var id = Guid.NewGuid();
                _mockService.Setup(s => s.Get(id)).Returns((Zone?)null);

                // Act
                var result = _controller.Get(id);

                // Assert
                result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Create_ShouldReturnCreatedResult()
        {
                // Arrange
                var zone = new Zone { Id = Guid.NewGuid(), Name = "New Zone" };

                // Act
                var result = _controller.Create(zone);

                // Assert
                var created = result as CreatedAtActionResult;
                created.Should().NotBeNull();
                created!.StatusCode.Should().Be(201);
                created.Value.Should().Be(zone);
                _mockService.Verify(s => s.Add(zone), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnNoContent_WhenZoneExists()
        {
                // Arrange
                var id = Guid.NewGuid();
                var existing = new Zone { Id = id, Name = "Old" };
                var updated = new Zone { Name = "Updated" };
                _mockService.Setup(s => s.Get(id)).Returns(existing);

                // Act
                var result = _controller.Update(id, updated);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                _mockService.Verify(s => s.Update(It.Is<Zone>(z => z.Id == id && z.Name == updated.Name)), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnNotFound_WhenZoneMissing()
        {
                // Arrange
                var id = Guid.NewGuid();
                var zone = new Zone { Name = "Doesn't Matter" };
                _mockService.Setup(s => s.Get(id)).Returns((Zone?)null);

                // Act
                var result = _controller.Update(id, zone);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
                _mockService.Verify(s => s.Update(It.IsAny<Zone>()), Times.Never);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent_WhenZoneExists()
        {
                // Arrange
                var id = Guid.NewGuid();
                var zone = new Zone { Id = id };
                _mockService.Setup(s => s.Get(id)).Returns(zone);

                // Act
                var result = _controller.Delete(id);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                _mockService.Verify(s => s.Remove(id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenZoneMissing()
        {
                // Arrange
                var id = Guid.NewGuid();
                _mockService.Setup(s => s.Get(id)).Returns((Zone?)null);

                // Act
                var result = _controller.Delete(id);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
                _mockService.Verify(s => s.Remove(It.IsAny<Guid>()), Times.Never);
        }
}

