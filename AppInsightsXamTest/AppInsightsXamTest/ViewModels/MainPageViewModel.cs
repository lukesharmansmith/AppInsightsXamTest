using System.Windows.Input;

using Microsoft.Extensions.Logging;

using Prism.Commands;
using Prism.Navigation;

namespace AppInsightsXamTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ILogger<MainPageViewModel> logger;

        public MainPageViewModel(INavigationService navigationService, ILogger<MainPageViewModel> logger)
            : base(navigationService)
        {
            this.logger = logger;

            this.Title = "Main Page";

            this.ButtonCommand = new DelegateCommand(this.OnButtonCommand);
        }

        public ICommand ButtonCommand { get; }

        private void OnButtonCommand()
        {
            this.logger.LogInformation("Hey something happened");
        }
    }
}
