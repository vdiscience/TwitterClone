using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneBackend.Services.Contracts
{
    public interface IProfileService
    {
        Task DeleteProfile(Guid id);
        Task<Profile> GetProfile(Guid id);
        Task<IEnumerable<Profile>> GetProfiles();
        Task<Profile> InsertProfile(Profile profile);
        Task UpdateProfile(Guid id, Profile profile);
    }
}