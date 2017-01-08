using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvxNuExample.Api;
using MvxNuExample.Api.Clients;

namespace MvxNuExample
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IWebApiClient>(new WebApiClient(Settings.NuApiBaseUrl));
            Mvx.RegisterSingleton<IAppClient>(new AppClient());

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}
