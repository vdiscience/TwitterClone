using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Contracts;

namespace TwitterClone.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DIProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public DIProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(Guid id)
        {
            var profile = await _profileService.GetProfile(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(Guid id, Profile profile)
        {
            try
            {
                if (id != profile.Id)
                {
                    return BadRequest();
                }

                await _profileService.InsertProfile(profile);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ProfileExists(Guid id)
        {
            var existProfile = _profileService.GetProfile(id);

            if (existProfile == null)
                throw new ArgumentException("Profile does not exist.");
            return existProfile != null;
        }
    }
}
