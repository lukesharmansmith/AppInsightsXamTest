using AppInsightsXamTest.ViewModels;
using AppInsightsXamTest.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Prism;
using Prism.Ioc;

using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AppInsightsXamTest
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterServices(s =>
            {
                s.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
            });
        }
    }
}
