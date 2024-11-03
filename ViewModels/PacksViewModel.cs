using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.objects.pack;
using CommunityToolkit.Maui.Views;

namespace Combat_Critters_2._0.ViewModels
{
    public class PacksViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private ObservableCollection<IPack> _allPacks;
        private ObservableCollection<IPack> _filteredPacks;

        private bool _hasPacks;
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

        private void OpenPack()
        {
            // var popup = new MaximizedCollectionViewPopup
            // {
            //     BindingContext = new PackCardViewModel()
            // };

            // Application.Current.MainPage.ShowPopup(popup);

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}