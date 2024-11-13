using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.MarketPlace.Interfaces;

namespace Combat_Critters_2._0.ViewModels
{
    public class MarketPlaceViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IVendor> _gameVendors;

        public ObservableCollection<IVendor> GameVendors
        {
            get => _gameVendors;
            set
            {
                _gameVendors = value;
                OnPropertyChanged(nameof(GameVendors));
            }
        }
        public MarketPlaceViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

            _gameVendors = new ObservableCollection<IVendor>();

            //Start Loading game packs
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadVendors();
        }

        private async Task LoadVendors()
        {

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}