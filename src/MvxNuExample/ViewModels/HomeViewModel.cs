using MvvmCross.Core.ViewModels;
using MvxNuExample.Api;
using MvxNuExample.Models;
using MvxNuExample.Utils;
using System.Threading.Tasks;

namespace MvxNuExample.ViewModels
{
    public class HomeViewModel 
        : BaseViewModel
    {
        private string _hello = "Hello MvvmCross";
        public string Hello
        { 
            get { return _hello; }
            set { SetProperty (ref _hello, value); }
        }

        private OptimizedObservableCollection<News> _newsItems;

        public OptimizedObservableCollection<News> NewsItems
        {
            get { return _newsItems; }
            set { SetProperty(ref _newsItems, value); }
        }

        private MvxCommand<News> openNewsItemCommand;

        public IMvxCommand OpenNewsItemCommand
        {
            get
            {
                return openNewsItemCommand = openNewsItemCommand ?? new MvxCommand<News>(ShowNewsItem);
            }
        }

        public HomeViewModel(IAppClient client) : base(client)
        {
            NewsItems = new OptimizedObservableCollection<News>();
            //NewsItems.Add(new News { title = "foo" });
        }

        public async Task Init()
        {
            await LoadFeed();
        }

        public async Task LoadFeed()
        {
            var list = await Client
                .FeedService
                .GetNewsFeedAsync(Fusillade.Priority.Explicit);

            if(list != null)
                NewsItems.AddRange(list);

            RaisePropertyChanged(() => NewsItems);
        }

        public void ShowNewsItem(News news)
        {
            ShowViewModel<NewsDetailViewModel, News> (news); 
        }
    }
}
