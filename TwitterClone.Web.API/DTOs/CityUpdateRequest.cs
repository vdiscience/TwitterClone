using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Web.API.DTOs;

public sealed class CityUpdateRequest
{
    [Required]
    [StringLength(100)]
    public string CityName { get; set; } = string.Empty;
}