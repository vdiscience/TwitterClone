namespace TwitterClone.Web.API.DTOs;

public sealed record CityResponse(
    Guid Id,
    string CityName,
    DateTime DateTimeEntered,
    bool IsDeleted,
    DateTime? DateTimeDeleted
);