using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;

namespace TwitterClone.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;
        private ILogger _logger;

        public CitiesController(DataContext context, ILogger<CitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private bool CityExists(Guid id)
        {
            try
            {
                var result = _context.Cities.Any(e => e.Id == id);
                if (id == null)
                    _logger.LogInformation($"City with id {id} not found.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities DbUpdateConcurrencyException error:", ex);
                throw;
            }
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    _logger.LogInformation($"City by id {id} not found.");
                    return NotFound();
                }
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities DbUpdateConcurrencyException error:", ex);
                throw;
            }

            _logger.LogInformation($"City id {id} deleted.");
            return NoContent();
        }

        // GET: api/Cities
        [HttpGet]
        //[Authorize]  //👈 new code
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            try
            {
                _logger.LogInformation("Search for Cities Started...");
                var result = await _context.Cities.ToListAsync();
                if (result == null)
                {
                    _logger.LogInformation("No records in Cities!");                    
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities critical error:", ex);
                throw;
            }
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    _logger.LogInformation($"City by id {id} not found.");
                    return NotFound();
                }
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities critical error:", ex);
                throw;
            }            
        }

        // POST: api/Cities
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize]  //👈 new code
        public async Task<ActionResult<City>> PostCity(City city)
        {
            if (city == null)
                _logger.LogInformation("No City information to update!");

            try
            {
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities DbUpdateConcurrencyException error:", ex);
                throw;
            }

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // PUT: api/Cities/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, City city)
        {
            try
            {
                if (id != city.Id)
                {
                    _logger.LogInformation($"Wrong City ID {id}.");
                    return BadRequest();
                }

                _context.Entry(city).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CityExists(id))
                {
                    _logger.LogInformation($"City by id {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogCritical("GetCities DbUpdateConcurrencyException error:", ex);
                    throw;
                }
            }

            return NoContent();
        }
    }
}
