using MvvmCross.WindowsUWP.Views;
using MvxNuExample.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MvxNuExample.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxRegion("PageContent")]
    public sealed partial class NewsDetailView : BaseView
    {
        public new NewsDetailViewModel ViewModel
        {
            get { return (NewsDetailViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public NewsDetailView()
        {
            this.InitializeComponent();
        }
    }
}
