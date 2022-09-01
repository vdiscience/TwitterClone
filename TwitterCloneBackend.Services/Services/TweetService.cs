using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneBackend.Services.Contracts;
using TwitterCloneBackend.Services.Handlers;

namespace TwitterCloneBackend.Services.Services
{
    public class TweetService : ITweetService
    {
        private DataContext _dataContext;

        public TweetService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Return all tweets.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Tweet>> GetAllTweets(PagingParameters pagingParameters, CancellationToken cancellationToken)
        {
            if (pagingParameters == null) throw new ArgumentNullException(nameof(pagingParameters));

            var tweets = await _dataContext.Tweets
                .OrderBy(on => on.DateTimeEntered)
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToListAsync(cancellationToken).ConfigureAwait(false);

            return tweets.Count == 0 ? throw new NullReferenceException() : tweets;
        }
    }
}
