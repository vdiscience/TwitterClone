using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Contracts;

namespace TwitterCloneBackend.Services.Services
{
    public class UsersService : IUsersService
    {
        private DataContext _dataContext;

        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// This method adds a new twitter user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddUser(User user)
        {
            var newUser = new User
            {
                UserName = user.UserName,
                Password = user.Password
            };

            _dataContext.Users.Add(newUser);
            await _dataContext.SaveChangesAsync();

            return newUser;
        }

        /// <summary>
        /// Update user password.
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        public async Task<User> UpdateUser(User updatedUser, CancellationToken cancellationToken)
        {
            var entityUser = _dataContext.Users.FirstOrDefault(u => u.UserName == updatedUser.UserName);

            entityUser.Password = updatedUser.Password;

            _dataContext.Users.Update(entityUser);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return entityUser;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _dataContext.Users.ToListAsync(cancellationToken).ConfigureAwait(false);

            if (users.Count == 0)
            {
                throw new NullReferenceException("Users not found!");
            }

            return users;
        }

        /// <summary>
        /// Get User by Id.
        /// </summary>
        /// <returns></returns>
        public Task<User> GetUserById(Guid Id)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                throw new NullReferenceException($"There is no user with Id {Id}");
            }

            return Task.FromResult(user);
        }


        /// <summary>
        /// Delete existing User.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<User> DeleteUser(Guid Id, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FindAsync(Id);

            if (user == null)
            {
                throw new NullReferenceException();
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return user;
        }
    }
}
