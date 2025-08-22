namespace TwitterClone.Web.API.Services.Models
{
    public record Result(bool Success, string? Error = null)
    {
        public static Result Ok() => new(true);
        public static Result Fail(string error) => new(false, error);
    }

    public record Result<T>(bool Success, T? Value, string? Error = null)
    {
        public static Result<T> Ok(T value) => new(true, value);
        public static Result<T> Fail(string error) => new(false, default, error);
    }

    public sealed record PagedResult<T>(
        IReadOnlyList<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize);
}