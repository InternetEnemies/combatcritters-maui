using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.deck;
using CombatCrittersSharp.objects.user;
using CombatCrittersSharp.rest;

namespace Combat_Critters_2._0.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private IDeck _featuredDeck;
        private bool _hasFeaturedDeck; //boolean for content triggers

        private ObservableCollection<IUser> _users;
        private bool _hasUsers; //boolean for content triggers

        private IUser _selectedUser;


        public IDeck FeaturedDeck
        {
            get => _featuredDeck;
            set
            {
                _featuredDeck = value;
                OnPropertyChanged(nameof(FeaturedDeck));
            }
        }
        public bool HasFeaturedDeck
        {
            get => _hasFeaturedDeck;
            set
            {
                _hasFeaturedDeck = value;
                OnPropertyChanged(nameof(HasFeaturedDeck));
            }
        }

        public ObservableCollection<IUser> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public bool HasUsers
        {
            get => _hasUsers;
            set
            {
                _hasUsers = value;
                OnPropertyChanged(nameof(HasUsers));
            }
        }

        public IUser SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ProfileViewModel()
        {

            //_featuredDeck = new IDeck();
            _hasFeaturedDeck = false;
            _users = new ObservableCollection<IUser>();
            _hasUsers = false;
            //_selectedUser = new User();

            //Subscribe to the FeaturedDeckChanged event
            DeckViewModel.FeaturedDeckChanged += OnFeaturedDeckChanged;
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

            Task.Run(async () => await InitializeProfileAsync());
        }

        private void OnFeaturedDeckChanged(IDeck featuredDeck)
        {
            HasFeaturedDeck = true;
            FeaturedDeck = featuredDeck; //Update this to reflect the change

            //Notify the UI that the FeaturedDeck has changed
            OnPropertyChanged(nameof(FeaturedDeck));
        }
        private async Task InitializeProfileAsync()
        {
            await LoadFeaturedDeck();
        }

        private async Task LoadFeaturedDeck()
        {
            try
            {
                var deck = await _backendService.GetFeaturedDeckAsync();
                if (deck != null)
                {
                    FeaturedDeck = deck;
                }
                else
                {
                    HasFeaturedDeck = false; //User has not set a featured deck
                }
            }
            catch (RestException ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load featured deck: {ex.Message}", "OK");
                }
            }
            catch (Exception)
            {
                throw; // bubble up to the global exception handler
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
