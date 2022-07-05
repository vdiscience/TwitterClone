using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneBackend.Services.Contracts;

public interface IProfileService
{
    /// <summary>
    /// Adds or updates a profile to the existing user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    /// <exception cref="AccessViolationException"></exception>
    Task<User> UpdateProfile(User user, Profile profile);
}