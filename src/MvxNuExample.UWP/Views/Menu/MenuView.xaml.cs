using MvvmCross.WindowsUWP.Views;
using MvxNuExample.UWP.Controls;
using MvxNuExample.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MvxNuExample.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxRegion("MenuContent")]
    public sealed partial class MenuView
    {
        public new MenuViewModel ViewModel
        {
            get { return (MenuViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MenuView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavMenuList.ItemsSource = new List<NavMenuItem>(
                new[]
                {
                    new NavMenuItem
                    {
                        Symbol = Symbol.Home,
                        Label = "Home",
                        Command = ViewModel.ShowHomeCommand
                    },
                    //new NavMenuItem
                    //{
                    //    Symbol = Symbol.Help,
                    //    Label = "Help",
                    //    Command = ViewModel.ShowHelpCommand
                    //},
                    //new NavMenuItem
                    //{
                    //    Symbol = Symbol.Setting,
                    //    Label = "Settings",
                    //    Command = ViewModel.ShowSettingCommand
                    //}
                });
        }

        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            item?.Command?.Execute(item.Parameters);
        }

        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }
    }
}
