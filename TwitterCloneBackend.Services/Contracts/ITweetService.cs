using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Handlers;

namespace TwitterCloneBackend.Services.Contracts;

public interface ITweetService
{
    /// <summary>
    /// Return all tweets.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Tweet>> GetAllTweets(PagingParameters pagingParameters, CancellationToken cancellationToken);
}