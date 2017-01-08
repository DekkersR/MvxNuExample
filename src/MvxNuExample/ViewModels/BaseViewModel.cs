using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvxNuExample.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvxNuExample.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        protected IAppClient Client;

        public BaseViewModel() { }

        public BaseViewModel(IAppClient client)
        {
            Client = client;
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public virtual string Title => string.Empty;

        protected bool ShowViewModel<TViewModel, TInit>(TInit parameter) where TViewModel : BaseViewModel<TInit>
        {
            var text = Mvx.Resolve<IMvxJsonConverter>().SerializeObject(parameter);
            return base.ShowViewModel<TViewModel>(new Dictionary<string, string> { { "parameter", text } });
        }
    }

    public abstract class BaseViewModel<TInit> : BaseViewModel
    {
        public async Task Init(string parameter)
        {
            var deserialized = Mvx.Resolve<IMvxJsonConverter>().DeserializeObject<TInit>(parameter);
            await RealInit(deserialized);
        }

        protected abstract Task RealInit(TInit parameter);
    }
}
