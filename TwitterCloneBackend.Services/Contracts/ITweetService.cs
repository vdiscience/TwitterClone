using TwitterCloneBackend.Entities.Models;
using TwitterCloneBackend.Services.Handlers;

namespace TwitterCloneBackend.Services.Contracts;

public interface ITweetService
{
    /// <summary>
    /// Return all tweet.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Tweet>> GetAllTweet(PagingParameters pagingParameters, CancellationToken cancellationToken);
}