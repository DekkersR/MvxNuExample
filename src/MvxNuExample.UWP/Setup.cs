using MvvmCross.WindowsUWP.Platform;
using MvvmCross.Core.ViewModels;
using Windows.UI.Xaml.Controls;
using MvvmCross.Platform;
using MvvmCross.WindowsUWP.Views;
using MvvmCross.Platform.Platform;
using MvxNuExample.UWP.Views;

namespace MvxNuExample.UWP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new MvxNuExample.App();
        }
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            return new CustomViewPresenter(rootFrame);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            //Mvx.RegisterSingleton<IDialogService>(() => new DialogService());
            Mvx.RegisterSingleton<MvxPresentationHint>(() => new MvxPanelPopToRootPresentationHint());
        }
    }
}
