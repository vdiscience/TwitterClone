using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Web.API.DTOs;
using TwitterClone.Web.API.Services;
using TwitterCloneBackend.Entities.Models;

namespace TwitterClone.Web.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CitiesController : ControllerBase
{
    #region Fields
    private readonly ICityService _service;
    private readonly ILogger<CitiesController> _logger;

    #endregion

    #region Constructor
    public CitiesController(ICityService service, ILogger<CitiesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    #endregion

    #region Public Methods
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<CityResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }
        else
        {
            pageNumber = pageNumber;
        }

        pageSize = Math.Clamp(pageSize, 1, 200);

        var result = await _service.GetCitiesAsync(pageNumber, pageSize, search, cancellationToken);

        Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
        Response.Headers["X-Page-Number"] = result.PageNumber.ToString();
        Response.Headers["X-Page-Size"] = result.PageSize.ToString();

        return Ok(result.Items);
    }

    [HttpGet("{id:guid}", Name = nameof(GetCity))]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityResponse>> GetCity(Guid id, CancellationToken cancellationToken = default)
    {
        var city = await _service.GetCityAsync(id, cancellationToken);

        if (city == null)
        {
            return NotFound();
        }
            
        return Ok(city);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CityResponse>> PostCity(
        [FromBody] CityCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateCityAsync(request, cancellationToken);

        if (!result.Success)
        {
            if (string.Equals(result.Error, "City name already exists.", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(request.CityName), result.Error!);
                return ValidationProblem(ModelState);
            }

            return Problem(result.Error);
        }

        return CreatedAtRoute(nameof(GetCity), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCity(Guid id, [FromBody] CityUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateCityAsync(id, request, cancellationToken);

        if (!result.Success)
        {
            if (result.Error == "NotFound")
            {
                return NotFound();
            }
                
            return Problem(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCity(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteCityAsync(id, cancellationToken);

        if (!result.Success)
        {
            if (result.Error == "NotFound")
            {
                return NotFound();
            }
                
            return Problem(result.Error);
        }

        return NoContent();
    }

    #endregion

    #region Private Methods
    private static CityResponse ToResponse(City c) =>
        new CityResponse(c.Id, c.CityName, c.DateTimeEntered, c.Deleted == 1, c.DateTimeDeleted == default ? null : c.DateTimeDeleted);

    #endregion
}
