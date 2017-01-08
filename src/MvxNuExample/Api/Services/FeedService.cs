using Akavache;
using Fusillade;
using MvxNuExample.Api.Clients;
using MvxNuExample.Api.Clients.Resolvers;
using MvxNuExample.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace MvxNuExample.Api.Services
{
    public interface IFeedService
    {
        Task<IEnumerable<News>> GetNewsFeedAsync(Priority priority = Priority.UserInitiated);
    }

    class FeedService : WebApiBaseService, IFeedService
    {
        private readonly IHttpResponseResolver _simpleJsonResponseResolver;

        public FeedService(IWebApiClient webApiClient) : base(webApiClient)
        {
            _simpleJsonResponseResolver = new SimpleJsonResponseResolver();
        }

        public async Task<IEnumerable<News>> GetNewsFeedAsync(Priority priority)
        {

            var cache = BlobCache.LocalMachine;
            //cache.Invalidate("newsfeed");
            var cachedFeed = cache.GetAndFetchLatest(
                "newsfeed",
                async () =>
                {
                    WebApiClient.Headers["Authorization"] = new AuthenticationHeaderValue("Basic", "").ToString();
                    WebApiClient.HttpResponseResolver = _simpleJsonResponseResolver;

                    return
                        await
                            ExecuteRemoteRequest(
                        () => WebApiClient.GetAsync<IEnumerable<News>>(priority, "v1.0/articles/top/?limit=50"));
                },
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 0, minutes: 0, seconds: 10);
                }
            );

            IEnumerable<News> result = null;
            cachedFeed.Subscribe(x => result = x);

            result = await cachedFeed.FirstOrDefaultAsync();
            return result;
        }
    }
}
