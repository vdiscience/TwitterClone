using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.Entities.Models;
using TwitterCloneBackend.Services.Services;
using TwitterCloneBackend.Tests.Repository;
using Xunit;

namespace TwitterCloneBackend.Tests.Services.Profiles
{
    public class ProfileServiceTests
    {
        [Fact]
        public async Task GetProfiles_ReturnsProfilesList()
        {
            // Arrange
            await DbContextService._dataContext.Profiles.AddRangeAsync(ProfilesRepo.Profiles.AsQueryable());
            await DbContextService._dataContext.SaveChangesAsync();

            var service = new ProfileService(DbContextService._dataContext);

            // Act
            var result = await service.GetProfiles();

            // Assert
            Assert.Equal(ProfilesRepo.Profiles.Count(), result.Count());
            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task GetProfiles_CheckProfilesIdList()
        {
            // Arrange
            await DbContextService._dataContext.Profiles.AddRangeAsync(ProfilesRepo.Profiles.AsQueryable());
            await DbContextService._dataContext.SaveChangesAsync();

            var service = new ProfileService(DbContextService._dataContext);

            // Act
            var result = await service.GetProfiles();

            // Assert
            Assert.Equal(ProfilesRepo.Profiles.OrderBy(x => x.Id), result.OrderBy(x => x.Id));

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

            await DbContextService._dataContext.Profiles.AddRangeAsync(profile);
            await DbContextService._dataContext.SaveChangesAsync();

            var service = new ProfileService(DbContextService._dataContext);

            // Act
            var result = await service.GetProfile(profile.Id);

            // Assert
            Assert.Equal(profile.Id, result.Id);
        }

        [Fact]
        public async Task GetProfile_NonExistingProfile_ThrowsException()
        {
            // Arrange
            var profileId = Guid.NewGuid();

            var service = new ProfileService(DbContextService._dataContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetProfile(profileId));
        }

        [Fact]
        public async Task UpdateProfile_UpdatesProfileWithMatchingId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var originalProfile = new Profile() { Id = id, ProfileName = "Test Profile" };
            var updatedProfile = new Profile() { Id = id, ProfileName = "Updated Profile" };

            var service = new ProfileService(DbContextService._dataContext);
            await service.InsertProfile(originalProfile);

            // Act
            // Detach the original profile from the context to avoid tracking conflicts
            DbContextService._dataContext.Entry(originalProfile).State = EntityState.Detached;

            await service.UpdateProfile(id, updatedProfile);

            // Assert
            var retrievedProfile = await service.GetProfile(id);
            Assert.Equal(updatedProfile.ProfileName, retrievedProfile.ProfileName);
        }

        [Fact]
        public async Task UpdateProfile_ThrowsArgumentExceptionIfIdAndProfileIdDoNotMatch()
        {
            // Arrange

            var profile = new Profile() { Id = Guid.NewGuid(), ProfileName = "Test Profile" };

            // Act
            var service = new ProfileService(DbContextService._dataContext);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await service.UpdateProfile(Guid.NewGuid(), profile));
        }

        [Fact]
        public async Task UpdateProfile_ThrowsArgumentExceptionIfProfileDoesNotExist()
        {
            // Arrange

            var id = Guid.NewGuid();
            var profile = new Profile() { Id = id, ProfileName = "Test Profile" };

            // Act
            var service = new ProfileService(DbContextService._dataContext);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.UpdateProfile(id, profile));
        }

        [Fact]
        public async Task InsertProfile_InsertsNewProfile()
        {
            // Arrange

            var profile = new Profile() { ProfileName = "Test Profile" };

            // Act
            var service = new ProfileService(DbContextService._dataContext);

            var insertedProfile = await service.InsertProfile(profile);

            // Assert

            Assert.NotNull(insertedProfile);
            Assert.Equal(profile.ProfileName, insertedProfile.ProfileName);
        }

        [Fact]
        public async Task DeleteProfile_DeletesProfileWithMatchingId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Create a new profile with the specified ID to be deleted
            var profileToDelete = new Profile() { Id = id, ProfileName = "Profile to Delete" };

            // Add the profile to the database
            await DbContextService._dataContext.Profiles.AddAsync(profileToDelete);
            await DbContextService._dataContext.SaveChangesAsync();

            // Act
            var service = new ProfileService(DbContextService._dataContext);

            // Delete the profile
            await service.DeleteProfile(id);

            // Assert
            var deletedProfile = await DbContextService._dataContext.Profiles.FindAsync(id);
            Assert.Null(deletedProfile);
        }

        [Fact]
        public async Task DeleteProfile_ThrowsArgumentExceptionIfProfileDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var service = new ProfileService(DbContextService._dataContext);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await service.DeleteProfile(id));
        }
    }
}
