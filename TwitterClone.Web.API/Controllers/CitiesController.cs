using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;

using Microsoft.AspNetCore.Authorization;  //👈 new code

namespace TwitterClone.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private ILogger _logger;

        private readonly DataContext _context;

        public CitiesController(DataContext context, ILogger<CitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cities
        [HttpGet]
        //[Authorize]  //👈 new code
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            _logger.LogInformation("GetCities Started...");
            var result = await _context.Cities.ToListAsync();

            try
            {
                if (result == null)
                {
                    _logger.LogInformation("GetCities is empty...");
                    return result;
                }
            }
            catch (Exception ex)
            {

                _logger.LogCritical("GetCities critical error:", ex);
            }
            return result;
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);

            try
            {
                if (city == null)
                {
                    _logger.LogInformation($"City by id {id} not found.");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                _logger.LogCritical("GetCities critical error:", ex);
            }
            return city;
        }

        // PUT: api/Cities/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, City city)
        {
            if (id != city.Id)
            {
                _logger.LogInformation($"Wrong City ID {id}.");
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
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

        // POST: api/Cities
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize]  //👈 new code
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                _logger.LogInformation($"City by id {id} not found.");
                return NotFound();
            }

            try
            {
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

        private bool CityExists(Guid id)
        {
            try
            {
                if (id != null)
                {
                    _logger.LogInformation($"City by id {id} found.");
                    return _context.Cities.Any(e => e.Id == id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetCities DbUpdateConcurrencyException error:", ex);
                throw;
            }
        }
    }
}
