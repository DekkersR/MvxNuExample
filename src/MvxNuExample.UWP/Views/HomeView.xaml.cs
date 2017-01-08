using MvvmCross.WindowsUWP.Views;
using MvxNuExample.Models;
using MvxNuExample.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MvxNuExample.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxRegion("PageContent")]
    public sealed partial class HomeView : BaseView
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public HomeView()
        {
            this.InitializeComponent();
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowNewsItem((sender as ListView).SelectedItem as News);
        }
    }
}
