using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.Entities;
using TwitterCloneBackend.Entities.Models;
using TwitterCloneBackend.Services.Services;

namespace TwitterClone.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfilesController(ProfileService profileService)
        {
            this._profileService = profileService;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<IEnumerable<Profile>> GetProfiles()
        {
            return await _profileService.GetProfiles();
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<Profile> GetProfile(Guid id)
        {
            return await _profileService.GetProfile(id);
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task PutProfile(Guid id, Profile profile)
        {
            await _profileService.UpdateProfile(id, profile);
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Profile> PostProfile(Profile profile)
        {
            return await _profileService.InsertProfile(profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task DeleteProfile(Guid id)
        {
            await _profileService.DeleteProfileAsync(id);
        }
    }
}
