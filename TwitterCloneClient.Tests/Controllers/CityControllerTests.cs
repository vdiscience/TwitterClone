using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterClone.Web.API.Controllers;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneClient.Tests.Controllers
{
    public class CitiesControllerTests
    {
        private readonly DataContext _dataContext;

        public CitiesControllerTests()
        {
            // Initialize a new instance of DataContext
            // This will create an in-memory database for testing
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataContext = new DataContext(options);
        }

        [Fact]
        public async Task PostCity_ReturnsCreatedResponse()
        {
            // Arrange
            var controller = new CitiesController(_dataContext);
            var city = new City { CityName = "New York" };

            // Act
            var result = await controller.PostCity(city);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCity = Assert.IsType<City>(createdAtActionResult.Value);
            Assert.Equal(city.CityName, returnedCity.CityName);
        }

        [Fact]
        public async Task GetCities_ReturnsAllCities()
        {
            // Arrange
            var cities = new List<City>
            {
                new City { Id = Guid.NewGuid(), CityName = "City 1" },
                new City { Id = Guid.NewGuid(), CityName = "City 2" },
                new City { Id = Guid.NewGuid(), CityName = "City 3" },
            };
            await _dataContext.Cities.AddRangeAsync(cities);
            await _dataContext.SaveChangesAsync();

            var controller = new CitiesController(_dataContext);

            // Act
            var result = await controller.GetCities();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<City>>>(result);
            var returnValue = Assert.IsType<List<City>>(actionResult.Value);
            Assert.Equal(cities.Count, returnValue.Count);
            Assert.Equal(3, returnValue.Count);
        }

        [Fact]
        public async Task GetCity_WithValidId_ReturnsCity()
        {
            // Arrange
            var city = new City { CityName = "City 1" };
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();

            var controller = new CitiesController(_dataContext);

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
            var controller = new CitiesController(_dataContext);

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
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();

            var updatedCity = new City { Id = city.Id, CityName = "Updated City 1" };

            var controller = new CitiesController(_dataContext);

            // Act
            var result = await controller.PutCity(city.Id, updatedCity);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedCityFromDb = await _dataContext.Cities.FindAsync(city.Id);
            Assert.Equal(updatedCity.CityName, updatedCityFromDb.CityName);
        }

        [Fact]
        public async Task PutCity_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var city = new City { Id = Guid.NewGuid(), CityName = "Test City" };
            _dataContext.Cities.Add(city);
            await _dataContext.SaveChangesAsync();
            var controller = new CitiesController(_dataContext);

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
            var controller = new CitiesController(_dataContext);

            // Act & Assert
            var result = await controller.GetCities();

            Assert.Equal(null, result.Result);
        }


        [Fact]
        public async Task PostCity_WithValidData_ReturnsCreatedAtActionResultWithCityObject()
        {
            // Arrange
            var controller = new CitiesController(_dataContext);
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
            var controller = new CitiesController(_dataContext);
            var city = new City { Id = Guid.NewGuid(), CityName = "New York" };
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();

            // Act
            city.CityName = "Updated City Name";
            var result = await controller.PutCity(city.Id, city);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedCity = await _dataContext.Cities.FindAsync(city.Id);
            Assert.Equal(city.CityName, updatedCity.CityName);
        }

        [Fact]
        public async Task DeleteCity_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new CitiesController(_dataContext);

            // Act
            var result = await controller.DeleteCity(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCity_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var controller = new CitiesController(_dataContext);
            var city = new City { Id = Guid.NewGuid(), CityName = "New York" };
            await _dataContext.Cities.AddAsync(city);
            await _dataContext.SaveChangesAsync();

            // Act
            var result = await controller.DeleteCity(city.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var deletedCity = await _dataContext.Cities.FindAsync(city.Id);
            Assert.Null(deletedCity);
        }
    }
}