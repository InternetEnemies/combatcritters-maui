using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.MarketPlace.Interfaces;

namespace Combat_Critters_2._0.ViewModels
{
    public class MarketPlaceViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IVendor> _gameVendors;
        private bool _hasVendors;

        private bool _isLoading;

        public bool HasVendors
        {
            get => _hasVendors;
            set
            {
                _hasVendors = value;
                OnPropertyChanged(nameof(HasVendors));
            }
        }
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

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
            HasVendors = false;

            //Start Loading game packs
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadVendors();
        }

        private async Task LoadVendors()
        {
            IsLoading = true;
            bool hasVendors = false;
            try
            {
                var vendors = await _backendService.GetVendorsAsync();

                if (vendors != null && vendors.Count > 0)
                {
                    GameVendors = new ObservableCollection<IVendor>(vendors);
                    hasVendors = true;
                }
                else
                {
                    GameVendors.Clear();
                }
            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load vendors. Please try again.", "OK");
            }
            catch (Exception)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
            }
            finally
            {
                //Set HasCards based on result of operation
                HasVendors = hasVendors;
                IsLoading = false;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}