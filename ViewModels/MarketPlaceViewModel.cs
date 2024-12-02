using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.MarketPlace.Implementations;
using CombatCrittersSharp.objects.MarketPlace.Interfaces;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Combat_Critters_2._0.ViewModels
{
    public class MarketPlaceViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private ObservableCollection<Vendor> _gameVendors;
        public ICommand VendorSelectedCommand { get; }
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

        public ObservableCollection<Vendor> GameVendors
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

            _gameVendors = new ObservableCollection<Vendor>();
            HasVendors = false;

            VendorSelectedCommand = new AsyncRelayCommand<Vendor>(OnVendorSelectedAsync);

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
                GameVendors = await _backendService.GetVendorsAsync();
                if (GameVendors.Count > 0)
                    hasVendors = true;
            }
            catch (InvalidOperationException e)
            {
                //Log
                Console.WriteLine(e.Message);

                var toast = Toast.Make(e.Message, ToastDuration.Short);
                await toast.Show();

            }
            catch (RestException e)
            {
                //Log 
                Console.WriteLine(e.Message);
                //Rest Exception
                var toast = Toast.Make("System Error", ToastDuration.Short);
                await toast.Show();

            }
            catch (AuthException e)
            {
                //Log
                Console.WriteLine(e.Message);
                //Auth Exception
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
                await toast.Show();
            }

            finally
            {
                IsLoading = false;
                HasVendors = hasVendors;
            }
        }

        private async Task OnVendorSelectedAsync(Vendor selectedVendor)
        {
            if (selectedVendor == null)
                return;

            //Debug
            var offerJson = await _backendService.GetAndLogVendorOfferAsync(selectedVendor.Id);
            Console.WriteLine("Raw JSON Response: " + offerJson);

            // Retrieve the list vendor offer
            List<Offer> offer = await _backendService.GetVendorOfferAsync(selectedVendor.Id);

            // // Create and show the popup
            var popup = new VendorDescriptionPopup(selectedVendor, offer);
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.ShowPopupAsync(popup);


        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}