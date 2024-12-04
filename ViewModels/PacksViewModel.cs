using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.pack;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.ViewModels
{
    public class PacksViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IPack> _allPacks;
        private ObservableCollection<IPack> _filteredPacks;
        private IPack _selectedPack;

        private bool _hasPacks;
        private bool _isLoading;


        public IPack SelectedPack
        {
            get => _selectedPack;
            set
            {
                _selectedPack = value;
                OnPropertyChanged(nameof(SelectedPack));
                GetContents();
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
        public ObservableCollection<IPack> AllPacks
        {
            get => _allPacks;
            set
            {
                _allPacks = value;
                OnPropertyChanged(nameof(AllPacks));
            }
        }

        public bool HasPacks
        {
            get => _hasPacks;
            set
            {
                _hasPacks = value;
                OnPropertyChanged(nameof(HasPacks));
            }
        }

        public ObservableCollection<IPack> FilteredPacks
        {
            get => _filteredPacks;
            set
            {
                _filteredPacks = value;
                OnPropertyChanged(nameof(FilteredPacks));
            }
        }

        private ObservableCollection<string> _gamePackImagesURL;
        public ObservableCollection<string> GamePackImagesURL
        {
            get => _gamePackImagesURL;
            set
            {
                _gamePackImagesURL = value;
                OnPropertyChanged(nameof(GamePackImagesURL));
            }
        }

        private ObservableCollection<ICard> _selectedPackContents;
        public ObservableCollection<ICard> SelectedPackContents
        {
            get => _selectedPackContents;
            set
            {
                _selectedPackContents = value;
                OnPropertyChanged(nameof(SelectedPackContents));
            }
        }


        public PacksViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _allPacks = new ObservableCollection<IPack>();
            _filteredPacks = new ObservableCollection<IPack>();
            _gamePackImagesURL = new ObservableCollection<string>();
            _selectedPackContents = new ObservableCollection<ICard>();
            HasPacks = false;

            //Start Loading game packs
            Task.Run(async () => await LoadDataNeeded());
        }

        private async Task LoadDataNeeded()
        {
            IsLoading = true;
            try
            {
                //Load Packs
                var packs = await _backendService.GetPacksAsync(); //Get all packs in the game

                if (packs != null && packs.Count > 0)
                {
                    AllPacks = new ObservableCollection<IPack>(packs);
                    FilteredPacks = new ObservableCollection<IPack>(packs);
                }
                else
                {
                    //Game has no Packs
                    AllPacks.Clear();
                }

                //Load Pack Images
                var root = "https://combatcritters.s3.us-east-1.amazonaws.com/";
                //Loading Game Pack Images
                for (int i = 0; i < 5; i++)
                {
                    var packURL = root + $"pack{i}.png";
                    GamePackImagesURL.Add(root + $"pack{i}.png");
                }
            }
            catch (InvalidOperationException)
            {
                var toast = Toast.Make("Access Denied. Contact Support.", ToastDuration.Short);
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

        public void FilterPacks(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                // Show all packs if search text is empty
                FilteredPacks = new ObservableCollection<IPack>(AllPacks);
            }
            else
            {
                //Filter packs based on the search text
                var filtered = AllPacks.Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                FilteredPacks = new ObservableCollection<IPack>(filtered);
            }
        }

        private async void GetContents()
        {
            try
            {
                var contents = await SelectedPack.GetPackContentsAsync();
                SelectedPackContents = new ObservableCollection<ICard>(contents);
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

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}