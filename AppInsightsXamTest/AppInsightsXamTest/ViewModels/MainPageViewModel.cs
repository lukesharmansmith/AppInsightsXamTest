using System;
using System.Windows.Input;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Prism.Commands;
using Prism.Navigation;

namespace AppInsightsXamTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IOptions<TelemetryConfiguration> config;
        private readonly ILogger<MainPageViewModel> logger;

        public MainPageViewModel(INavigationService navigationService, IOptions<TelemetryConfiguration> config, ILogger<MainPageViewModel> logger)
            : base(navigationService)
        {
            this.config = config;
            this.logger = logger;

            this.Title = "Main Page";

            this.ButtonCommand = new DelegateCommand<string>(this.OnButtonCommand);
        }

        public ICommand ButtonCommand { get; }

        private void OnButtonCommand(string commandType)
        {
            switch (commandType)
            {
                case "Info":
                    this.logger.LogInformation("Infomational message");
                    break;
                case "Warn":
                    this.logger.LogWarning("Somethings gone wrong but we will keep calm and carry on");
                    break;
                case "Error":
                    this.logger.LogError("Ive got a bad feeling about this");
                    break;
                case "RealError":
                    this.logger.LogError(new InvalidOperationException("Game over man"), "Game over man, Game over.");
                    break;
                case "Crit":
                    this.logger.LogCritical("Nuke it from orbit, its the only way");
                    break;

                default:
                    break;
            }
        }
    }
}
