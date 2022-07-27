using AppInsightsXamTest.ViewModels;
using AppInsightsXamTest.Views;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Prism;
using Prism.Ioc;

using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AppInsightsXamTest
{
    public partial class App
    {
        private TelemetryClient telemetryClient;

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            this.telemetryClient = this.Container.Resolve<TelemetryClient>();

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
                    logging.AddApplicationInsights(
                        configureTelemetryConfiguration: config =>
                        {
                            config.ConnectionString = "";
                        },
                        configureApplicationInsightsLoggerOptions: (options) => { });

                    logging.SetMinimumLevel(LogLevel.Information);
                });

                s.AddSingleton(provider =>
                {
                    var config = provider.GetService<IOptions<TelemetryConfiguration>>();
                    return new TelemetryClient(config.Value);
                });
            });
        }

        protected override void OnSleep()
        {
            // If going background then force a telemetry send.
            if(this.telemetryClient != null)
            {
                this.telemetryClient.Flush();
            }

            base.OnSleep();
        }
    }
}
