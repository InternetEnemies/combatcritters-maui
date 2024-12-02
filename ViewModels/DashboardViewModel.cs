using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;

using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;

        public ICommand OpenGitHubCommand { get; }


        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        /// <summary>
        /// Constructor for the Dashboard View Model
        /// </summary>
        /// <param name="username">Client Username is passed Upon creation</param>
        public DashboardViewModel(string username)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _username = username;

            OpenGitHubCommand = new Command(OpenGitHub);
        }

        private void OpenGitHub()
        {
            var uri = new Uri("https://github.com/InternetEnemies/combatcritters-maui");
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.External);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}