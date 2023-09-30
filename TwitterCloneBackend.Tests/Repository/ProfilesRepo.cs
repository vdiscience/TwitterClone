using TwitterCloneBackend.Entities.Models;

namespace TwitterCloneBackend.Tests.Repository
{
    public static class ProfilesRepo
    {
        public static List<Profile> Profiles { get; set; }
        static ProfilesRepo()
        {
            Profiles = new List<Profile>
            {
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 1" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 2" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 3" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 4" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 5" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 6" },
            };
        }
    }
}
