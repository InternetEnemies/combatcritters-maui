using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.user;

namespace Combat_Critters_2._0.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private int _cardCount;
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

        public int CardCount
        {
            get => _cardCount;
            set
            {
                _cardCount = value;
                OnPropertyChanged(nameof(CardCount));
            }
        }
        /// <summary>
        /// Constructor for the Dashboard View Model
        /// </summary>
        /// <param name="username">Client Username is passed Upon creation</param>
        public DashboardViewModel(string username)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            Username = username;

            Task.Run(async () => LoadCardCount());
        }

        private async void LoadCardCount()
        {
            try
            {
                //CardCount = await _backendService.GetCardCountAsync();
            }
            catch (Exception ex)
            {

            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}