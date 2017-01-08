namespace MvxNuExample.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public void ShowMenu()
        {
            ShowViewModel<HomeViewModel>();
            ShowViewModel<MenuViewModel>();
        }

        public void ShowHome()
        {
            ShowViewModel<HomeViewModel>();
        }

    }
}
