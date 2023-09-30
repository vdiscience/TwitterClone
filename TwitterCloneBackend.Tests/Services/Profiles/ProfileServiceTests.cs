using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Services;
using Xunit;

namespace TwitterCloneBackend.Tests.Services.Profiles
{
    public class ProfileServiceTests
    {
        private readonly DataContext _dataContext;

        public ProfileServiceTests()
        {
            // Initialize a new instance of DataContext
            // This will create an in-memory database for testing
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataContext = new DataContext(options);
        }

        [Fact]
        public async Task GetProfiles_ReturnsProfilesList()
        {
            // Arrange
            var profiles = new List<Profile>
            {
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 1" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 2" },
            }.AsQueryable();

            await _dataContext.Profiles.AddRangeAsync(profiles);
            await _dataContext.SaveChangesAsync();

            var service = new ProfileService(_dataContext);

            // Act
            var result = await service.GetProfiles();

            // Assert
            Assert.Equal(profiles.Count(), result.Count());
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProfiles_CheckProfilesIdList()
        {
            // Arrange
            var profiles = new List<Profile>
            {
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 1" },
                new Profile { Id = Guid.NewGuid(), ProfileName = "Profile 2" },
            }.AsQueryable();

            await _dataContext.Profiles.AddRangeAsync(profiles);
            await _dataContext.SaveChangesAsync();

            var service = new ProfileService(_dataContext);

            // Act
            var result = await service.GetProfiles();

            // Assert
            // Check the order and matching of Id properties
            Assert.Equal(profiles.OrderBy(x => x.Id), result.OrderBy(x => x.Id));

            #region AlsoRight
            //bool areEqual = true;

            //foreach (var itemProfile in profiles.OrderBy(x=>x.Id))
            //{
            //    bool foundMatch = false;
            //    foreach (var itemResult in result.OrderBy(x => x.Id))
            //    {
            //        if (itemResult.Id == itemProfile.Id)
            //        {
            //            foundMatch = true;
            //            break;
            //        }
            //    }
            //    if (!foundMatch)
            //    {
            //        areEqual = false;
            //        break;
            //    }
            //}

            //Assert.True(areEqual, "Not all items in profiles have a matching item in result.");


            //// Assuming the two collections: profiles and result
            //bool areEqual1Linq = profiles.All(itemProfile =>
            //    result.AsQueryable().Any(itemResult => itemResult.Id == itemProfile.Id)
            //);

            //// Assert that all items in profiles have a matching item in result
            //Assert.True(areEqual1Linq, "Not all items in profiles have a matching item in result.");

            #endregion AlsoRight
        }

        [Fact]
        public async Task GetProfile_ExistingProfile_ReturnsProfile()
        {
            // Arrange
            var profileId = Guid.NewGuid();
            var profile = new Profile { Id = profileId, ProfileName = "Profile 1" };

            await _dataContext.Profiles.AddRangeAsync(profile);
            await _dataContext.SaveChangesAsync();

            var service = new ProfileService(_dataContext);

            // Act
            var result = await service.GetProfile(profile.Id);

            // Assert
            Assert.Equal(profile.Id, result.Id);
        }

        //[Fact]
        //public async Task GetProfile_NonExistingProfile_ThrowsException()
        //{
        //    // Arrange
        //    var profileId = Guid.NewGuid();

        //    var mockContext = new Mock<DataContext>();
        //    _mockDataContext.Setup(c => c.Profiles.FindAsync(profileId)).ReturnsAsync((Profile)null);

        //    var profileService = new ProfileService(mockContext.Object);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<ArgumentException>(() => profileService.GetProfile(profileId));
        //}

        //[Fact]
        //public async Task UpdateProfile_UpdatesProfileWithMatchingId()
        //{
        //    // Arrange

        //    var id = Guid.NewGuid();
        //    var profile = new Profile() { Id = id, ProfileName = "Test Profile" };

        //    // TODO: Arrange for the DataContext to update the profile successfully

        //    // Act

        //    await _profileService.UpdateProfile(id, profile);

        //    // Assert

        //    // TODO: Assert that the profile was updated successfully
        //}

        //[Fact]
        //public async Task UpdateProfile_ThrowsArgumentExceptionIfIdAndProfileIdDoNotMatch()
        //{
        //    // Arrange

        //    var profile = new Profile() { Id = Guid.NewGuid(), ProfileName = "Test Profile" };

        //    // Act

        //    // Assert

        //    Assert.ThrowsAsync<ArgumentException>(async () => await _profileService.UpdateProfile(Guid.NewGuid(), profile));
        //}

        //[Fact]
        //public async Task UpdateProfile_ThrowsArgumentExceptionIfProfileDoesNotExist()
        //{
        //    // Arrange

        //    var id = Guid.NewGuid();
        //    var profile = new Profile() { Id = id, ProfileName = "Test Profile" };

        //    // TODO: Arrange for the DataContext to throw a DbUpdateConcurrencyException when updating the profile

        //    // Act

        //    // Assert

        //    Assert.ThrowsAsync<ArgumentException>(async () => await _profileService.UpdateProfile(id, profile));
        //}

        //[Fact]
        //public async Task InsertProfile_InsertsNewProfile()
        //{
        //    // Arrange

        //    var profile = new Profile() { ProfileName = "Test Profile" };

        //    // TODO: Arrange for the DataContext to insert the profile successfully

        //    // Act

        //    var insertedProfile = await _profileService.InsertProfile(profile);

        //    // Assert

        //    Assert.NotNull(insertedProfile);
        //    Assert.Equal(profile.ProfileName, insertedProfile.ProfileName);
        //}

        //[Fact]
        //public async Task DeleteProfile_DeletesProfileWithMatchingId()
        //{
        //    // Arrange

        //    var id = Guid.NewGuid();

        //    // TODO: Arrange for the DataContext to delete the profile successfully

        //    // Act

        //    await _profileService.DeleteProfile(id);

        //    // Assert

        //    // TODO: Assert that the profile was deleted successfully
        //}

        //[Fact]
        //public async Task DeleteProfile_ThrowsArgumentExceptionIfProfileDoesNotExist()
        //{
        //    // Arrange

        //    var id = Guid.NewGuid();

        //    // TODO: Arrange for the DataContext to return null when finding the profile by id

        //    // Act

        //    // Assert

        //    Assert.ThrowsAsync<ArgumentException>(async () => await _profileService.DeleteProfile(id));
        //}
    }
}
