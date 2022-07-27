using System;
using System.IO;

using AppInsightsXamTest.ViewModels;
using AppInsightsXamTest.Views;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
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

        private ILogger<App> logger;

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            this.telemetryClient = this.Container.Resolve<TelemetryClient>();
            this.logger = this.Container.Resolve<ILogger<App>>();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterServices(s =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "telemetryStore");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }  

                // Server channel allows us to save telemetry to a file location to handle offline scenarios, name is slightly misleading.
                var channel = new ServerTelemetryChannel
                {
                    StorageFolder = path
                };

                s.Configure<TelemetryConfiguration>(config => 
                {
                    config.TelemetryChannel = channel;
                    channel.Initialize(config);
                });

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

                    logging.SetMinimumLevel(LogLevel.Debug);
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
                this.logger.LogDebug("Going to sleep Zzzzzzz (Flushing telemetry)");
                this.telemetryClient.Flush();
            }

            base.OnSleep();
        }
    }
}
