using System;
using System.Windows.Input;

using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

using Prism.Commands;
using Prism.Navigation;

namespace AppInsightsXamTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly TelemetryClient telemetryClient;
        private readonly ILogger<MainPageViewModel> logger;

        private string customMessage;

        /// Standard <see cref="ILogger{TCategoryName}"/> and <see cref="TelemetryClient"/> can be injected using DI.
        public MainPageViewModel(INavigationService navigationService, TelemetryClient telemetryClient, ILogger<MainPageViewModel> logger)
            : base(navigationService)
        {
            this.telemetryClient = telemetryClient;
            this.logger = logger;

            this.Title = "Main Page";

            this.ButtonCommand = new DelegateCommand<string>(this.OnButtonCommand);
        }

        public ICommand ButtonCommand { get; }

        public string CustomMessage
        {
            get => this.customMessage;
            set
            {
                this.SetProperty(ref this.customMessage, value);
            }
        }

        private void OnButtonCommand(string commandType)
        {
            switch (commandType)
            {
                case "Info":
                    this.logger.LogInformation(string.IsNullOrWhiteSpace(this.CustomMessage) ? "Infomational message" : this.CustomMessage);
                    break;
                case "Warn":
                    this.logger.LogWarning(string.IsNullOrWhiteSpace(this.CustomMessage) ? "Somethings gone wrong but we will keep calm and carry on" : this.CustomMessage);
                    break;
                case "Error":
                    this.logger.LogError(string.IsNullOrWhiteSpace(this.CustomMessage) ? "Ive got a bad feeling about this" : this.CustomMessage);
                    break;
                case "RealError":
                    this.logger.LogError(new InvalidOperationException("Game over man"), "Game over man, Game over.");
                    break;
                case "Crit":
                    this.logger.LogCritical(string.IsNullOrWhiteSpace(this.CustomMessage) ? "Nuke it from orbit, its the only way" : this.CustomMessage);
                    break;
                case "CustomEvent":
                    this.telemetryClient.TrackEvent("CustomEventA");
                    break;
                default:
                    break;
            }
        }
    }
}
