using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Contracts;

namespace TwitterCloneBackend.Services.Services
{
    public class ProfileService : IProfileService
    {
        private DataContext _context;

        public ProfileService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profile>> GetProfiles()
        {
            return await _context.Profiles.ToListAsync();
        }

        public async Task<Profile> GetProfile(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                throw new ArgumentException("Profile not found!");
            }

            return profile;
        }

        public async Task UpdateProfile(Guid id, Profile profile)
        {
            if (id != profile.Id)
            {
                throw new ArgumentException("ID and the Supplied Profile ID don't match.");
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    throw new ArgumentException("Profile does not exist.");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Profile> InsertProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task DeleteProfile(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                throw new ArgumentException("Profile Id does not exist.");
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }

        private bool ProfileExists(Guid id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
