using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;

        private ObservableCollection<ICard> _gameCards;
        public ObservableCollection<ICard> GameCards
        {
            get => _gameCards;
            set
            {
                _gameCards = value;
                OnPropertyChanged(nameof(GameCards));
            }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
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
            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }
        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
        }
        public async Task LoadGameCards()
        {
            IsLoading = true;
            try
            {
                //Update Game Cards
                GameCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

            }
            catch (InvalidOperationException)
            {
                //If this happens, either client instance is null of user instance of client is null
                //Display popup
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();

            }
            catch (ArgumentNullException)
            {
                //If this happens, the argument for card Query is null
                var toast = Toast.Make("Invalid Card Query", ToastDuration.Short);
                await toast.Show();

            }
            catch (RestException)
            {
                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException)
            {
                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }

            finally
            {
                IsLoading = false;
            }
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