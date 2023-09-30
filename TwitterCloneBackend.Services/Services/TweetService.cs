using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.Entities;
using TwitterCloneBackend.Entities.Models;
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
        /// Return all tweet.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Tweet>> GetAllTweet(PagingParameters pagingParameters, CancellationToken cancellationToken)
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
