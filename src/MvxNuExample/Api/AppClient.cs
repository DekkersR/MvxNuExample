using MvvmCross.Platform;
using MvxNuExample.Api.Services;
using System;

namespace MvxNuExample.Api
{
    public interface IAppClient
    {
        IFeedService FeedService { get; }
    }

    public class AppClient : IAppClient
    {
        private readonly Lazy<IFeedService> _feedService = new Lazy<IFeedService>(Mvx.Resolve<IFeedService>);

        public IFeedService FeedService => _feedService.Value;
    }
}
