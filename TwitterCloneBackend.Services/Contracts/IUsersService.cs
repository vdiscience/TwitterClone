using TwitterCloneBackend.DDD.Models;

namespace TwitterCloneBackend.Services.Contracts;

public interface IUsersService
{
    /// <summary>
    /// This method adds a new twitter user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<User> AddUser(User user);

    /// <summary>
    /// Update user password.
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    Task<User> UpdateUser(User updatedUser, CancellationToken cancellationToken);

    Task<List<User>> GetUsers(CancellationToken cancellationToken);

    /// <summary>
    /// Get User by Id.
    /// </summary>
    /// <returns></returns>
    Task<User> GetUserById(Guid Id);

    /// <summary>
    /// Delete existing User.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    Task<User> DeleteUser(Guid Id, CancellationToken cancellationToken);
}