using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxNuExample.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public IMvxCommand ShowHomeCommand => new MvxCommand(OnShowHomeCommand);

        private void OnShowHomeCommand()
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}
