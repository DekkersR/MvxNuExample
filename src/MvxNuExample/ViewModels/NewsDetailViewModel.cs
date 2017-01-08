using MvxNuExample.Models;
using System.Threading.Tasks;

namespace MvxNuExample.ViewModels
{
    public class NewsDetailViewModel : BaseViewModel<News>
    {
        public News Item { get; set; }

        protected override async Task RealInit(News parameter)
        {
            Item = parameter;
        }
    }
}
