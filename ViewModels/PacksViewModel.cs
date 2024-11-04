using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Pages.Popups;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.pack;
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
        public ICommand OpenPackCommand { get; }
        //Constructor
        public PacksViewModel()
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _allPacks = new ObservableCollection<IPack>();
            _filteredPacks = new ObservableCollection<IPack>();
            HasPacks = false;
            OpenPackCommand = new Command(OpenPack);

            //Start Loading game packs
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadPacks();
        }

        private async Task LoadPacks()
        {
            IsLoading = true;
            bool hasPacks = false; //function scoped variable

            try
            {
                var packs = await _backendService.GetPacksAsync(); //Get all packs in the game

                if (packs != null && packs.Count > 0)
                {
                    AllPacks = new ObservableCollection<IPack>(packs);
                    FilteredPacks = new ObservableCollection<IPack>(packs);
                    hasPacks = true;
                }
                else
                {
                    //Game has no Packs
                    AllPacks.Clear();
                }

            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load users. Please try again.", "OK");
            }
            finally
            {
                HasPacks = hasPacks;
                IsLoading = false; //Turn off loading indicator
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

        private async void OpenPack()
        {
            if (Application.Current?.MainPage != null)
            {

                if (SelectedPack == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No pack selected.", "OK");
                    return;
                }

                try
                {
                    //Fetch cards in the selected pack
                    Console.WriteLine("Opening pack..");

                    var cards = await SelectedPack.GetPackContentsAsync();
                    Console.WriteLine($"Opening pack..{cards.Count}");

                    //Create and show the popup with the selected cards
                    var popup = new PackPopup(new ObservableCollection<ICard>(cards));
                    await Application.Current.MainPage.ShowPopupAsync(popup);


                }
                catch (RestException)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to open pack. Please try again.", "OK");
                }

            }



        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}