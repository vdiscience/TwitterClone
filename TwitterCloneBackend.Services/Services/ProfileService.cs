using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Contracts;

namespace TwitterCloneBackend.Services.Services
{
    public class ProfileService : IProfileService
    {
        private DataContext _dataContext;

        public ProfileService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Adds or updates a profile to the existing user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        /// <exception cref="AccessViolationException"></exception>
        public async Task<User> UpdateProfile(User user, Profile profile)
        {
            var currentUser = _dataContext.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if (currentUser == null)
            {
                throw new AccessViolationException("You cannot add a new profile to this user!");
            }

            currentUser.Profile = profile;

            _dataContext.Users.Update(currentUser);
            await _dataContext.SaveChangesAsync();

            return currentUser;
        }
    }
}
