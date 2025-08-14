using Microsoft.AspNetCore.Mvc;
using TwitterClone.Web.API.Controllers;
using TwitterCloneBackend.Entities.Models;
using TwitterCloneBackend.Tests.Repository;
using Xunit;

namespace TwitterCloneBackend.Tests.Controllers.Cities
{
    public class CitiesControllerTests
    {
        [Fact]
        public async Task GetCities_ReturnsAllCities()
        {
            // Arrange
            DbContextService._dataContext.Cities.RemoveRange(DbContextService._dataContext.Cities);
            await DbContextService._dataContext.SaveChangesAsync();
            await DbContextService._dataContext.Cities.AddRangeAsync(CitiesRepo.Cities.AsQueryable());
            await DbContextService._dataContext.SaveChangesAsync();

            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.GetCities();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<City>>>(result);
            var returnValue = Assert.IsType<List<City>>(actionResult.Value);

            Assert.Equal(CitiesRepo.Cities.Count, returnValue.Count);
            Assert.Equal(6, returnValue.Count); // Total of 6 cities in the db. 6 can change.
        }

        [Fact]
        public async Task PostCity_ReturnsCreatedResponse()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);
            var city = new City { CityName = "New York" };

            // Act
            var result = await controller.PostCity(city);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCity = Assert.IsType<City>(createdAtActionResult.Value);

            Assert.Equal(city.CityName, returnedCity.CityName);
        }

        [Fact]
        public async Task GetCity_WithValidId_ReturnsCity()
        {
            // Arrange
            var city = new City { CityName = "City 1" };
            await DbContextService._dataContext.Cities.AddAsync(city);
            await DbContextService._dataContext.SaveChangesAsync();

            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.GetCity(city.Id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<City>>(result);
            var returnValue = Assert.IsType<City>(actionResult.Value);
            Assert.Equal(city.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetCity_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.GetCity(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PutCity_WithValidId_UpdatesCity()
        {
            // Arrange
            var city = new City { CityName = "City 1" };
            await DbContextService._dataContext.Cities.AddAsync(city);
            await DbContextService._dataContext.SaveChangesAsync();

            var updatedCity = new City { Id = city.Id, CityName = "Updated City 1" };

            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.PutCity(city.Id, updatedCity);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedCityFromDb = await DbContextService._dataContext.Cities.FindAsync(city.Id);
            Assert.NotNull(updatedCityFromDb);
            Assert.Equal(updatedCity.CityName, updatedCityFromDb!.CityName);
        }

        [Fact]
        public async Task PutCity_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var city = new City { Id = Guid.NewGuid(), CityName = "Test City" };
            DbContextService._dataContext.Cities.Add(city);
            await DbContextService._dataContext.SaveChangesAsync();
            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.PutCity(Guid.NewGuid(), city);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetCities_ReturnsNull()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);

            // Act & Assert
            var result = await controller.GetCities();

            Assert.Null(result.Result);
        }

        [Fact]
        public async Task PostCity_WithValidData_ReturnsCreatedAtActionResultWithCityObject()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);
            var city = new City
            {
                CityName = "New York",
            };

            // Act
            var result = await controller.PostCity(city);

            // Assert
            // var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdCity = Assert.IsType<City>(okResult.Value);
            Assert.Equal(city.CityName, createdCity.CityName);
            Assert.Equal(city.CityName, createdCity.CityName);
        }

        [Fact]
        public async Task PutCity_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);
            var city = new City { Id = Guid.NewGuid(), CityName = "New York" };

            await DbContextService._dataContext.Cities.AddAsync(city);
            await DbContextService._dataContext.SaveChangesAsync();

            // Act
            city.CityName = "Updated City Name";
            var result = await controller.PutCity(city.Id, city);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedCity = await DbContextService._dataContext.Cities.FindAsync(city.Id);
            Assert.NotNull(updatedCity);
            Assert.Equal(city.CityName, updatedCity!.CityName);
        }

        [Fact]
        public async Task DeleteCity_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);

            // Act
            var result = await controller.DeleteCity(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCity_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var controller = new CitiesController(DbContextService._dataContext);
            var city = new City { Id = Guid.NewGuid(), CityName = "New York" };
            await DbContextService._dataContext.Cities.AddAsync(city);
            await DbContextService._dataContext.SaveChangesAsync();

            // Act
            var result = await controller.DeleteCity(city.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var deletedCity = await DbContextService._dataContext.Cities.FindAsync(city.Id);
            Assert.Null(deletedCity);
        }
    }
}
