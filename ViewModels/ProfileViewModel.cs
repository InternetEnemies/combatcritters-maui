using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
using CombatCrittersSharp.objects.user;
using CombatCrittersSharp.rest;

namespace Combat_Critters_2._0.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private BackendService _backendService;
        private IDeck _featuredDeck;
        private ObservableCollection<ICard> _featuredDeckCard;

        private bool _hasFeaturedDeck; //boolean for content triggers

        private ObservableCollection<IUser> _users;
        private bool _hasUsers; //boolean for content triggers

        private IUser _selectedUser;

        public ObservableCollection<ICard> FeaturedDeckCards
        {
            get => _featuredDeckCard;
            set
            {
                _featuredDeckCard = value;
                OnPropertyChanged(nameof(FeaturedDeckCards));
            }
        }

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
            _featuredDeckCard = new ObservableCollection<ICard>();
            //_selectedUser = new User();

            //Subscribe to the FeaturedDeckChanged event
            DeckViewModel.FeaturedDeckChanged += OnFeaturedDeckChanged;
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

            Task.Run(async () => await InitializeProfileAsync());
        }

        private async Task LoadFeaturedDeckCards()
        {
            if (FeaturedDeck != null)
            {
                try
                {
                    var deckCards = await FeaturedDeck.GetCards();

                    if (deckCards != null && deckCards.Count > 0)
                    {
                        FeaturedDeckCards = new ObservableCollection<ICard>((IEnumerable<ICard>)deckCards);
                    }
                    else
                    {
                        //no cards found, clear the collection
                        FeaturedDeckCards.Clear();
                    }
                }
                catch (RestException ex)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load cards for featured deck: {ex.Message}", "OK");
                    }
                }
                catch (Exception ex)
                {
                    throw; //bubble up the exception to the global handler
                }
            }
        }

        private async void OnFeaturedDeckChanged(IDeck featuredDeck)
        {
            //Update the featured deck 
            HasFeaturedDeck = true;
            FeaturedDeck = featuredDeck; //Update this to reflect the change

            //Get the featured deck cards
            await LoadFeaturedDeckCards();

        }
        private async Task InitializeProfileAsync()
        {
            //Fetch the user featured deck and deck cards
            await LoadFeaturedDeck();
            await LoadFeaturedDeckCards();
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
