using TwitterCloneBackend.Entities.Models;

namespace TwitterCloneBackend.Tests.Repository
{
    public static class CitiesRepo
    {
        public static List<City> Cities { get; set; }
        static CitiesRepo()
        {
            Cities = new List<City>
            {
                new City { Id = Guid.NewGuid(), CityName = "City 1" },
                new City { Id = Guid.NewGuid(), CityName = "City 2" },
                new City { Id = Guid.NewGuid(), CityName = "City 3" },
                new City { Id = Guid.NewGuid(), CityName = "City 4" },
                new City { Id = Guid.NewGuid(), CityName = "City 5" },
                new City { Id = Guid.NewGuid(), CityName = "City 6" },
            };
        }
    }
}
