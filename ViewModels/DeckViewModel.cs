using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;
using CommunityToolkit.Maui.Alerts;

namespace Combat_Critters_2._0.ViewModels
{
    public class DeckViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;
        private ObservableCollection<IDeck> _userDecks;
        private ObservableCollection<ICard> _selectedDecksCards;
        private IDeck _selectedDeck;
        public ICommand CreateDeckCommand { get; }
        public ICommand DeckSelectedCommand { get; set; }
        public ICommand FeatureOnProfileCommand { get; }
        public ICommand DeleteDeckCommand { get; }
        private bool _hasDecks;

        public ObservableCollection<ICard> SelectedDecksCards
        {
            get => _selectedDecksCards;
            set
            {
                _selectedDecksCards = value;
                OnPropertyChanged(nameof(SelectedDecksCards));
            }
        }
        public IDeck SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (_selectedDeck != value)
                {
                    _selectedDeck = value;
                    OnPropertyChanged(nameof(SelectedDeck));
                }

                if (DeckSelectedCommand.CanExecute(_selectedDeck))
                    DeckSelectedCommand.Execute(_selectedDeck);
            }
        }
        public ObservableCollection<IDeck> UserDecks
        {
            get => _userDecks;
            set
            {
                _userDecks = value;
                Console.Write("UserDecks property changed");
                OnPropertyChanged(nameof(UserDecks));
            }
        }

        public bool HasDecks
        {
            get => _hasDecks;
            set
            {
                _hasDecks = value;
                OnPropertyChanged(nameof(HasDecks));
            }
        }

        public DeckViewModel()
        {
            _userDecks = new ObservableCollection<IDeck>();
            _selectedDecksCards = new ObservableCollection<ICard>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _selectedDeck = new Deck(null, null, -1, "");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));

            CreateDeckCommand = new Command(OnCreateDeckCommand);
            DeckSelectedCommand = new Command<IDeck>(OnDeckSelected);
            FeatureOnProfileCommand = new Command<IDeck>(FeatureOnProfile);
            DeleteDeckCommand = new Command(DeleteDeck);
            _hasDecks = false;

            //start Loading the user decks.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async void FeatureOnProfile(IDeck deck)
        {
            if (deck != null)
            {
                try
                {
                    Console.WriteLine($"Featuring deck {deck.Name} on profile...");
                    await _backendService.FeatureDeckOnProfileAsync(deck);
                    Console.WriteLine($"Deck {deck.Name} has been featured on the profile");

                    //Display confirmation UI
                    var toast = Toast.Make($"Deck '{deck.Name}' has been featured on your profile.", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                }
                catch (RestException ex)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to feature deck: {ex.Message}", "OK");
                    }
                }
                catch (Exception)
                {
                    throw; // bubble up to the global exception handler
                }
            }
            else
            {
                //No deck selected or invalid deck
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Open the deck you wish to feature first", "OK");
                }
            }

        }

        private void DeleteDeck()
        {
            //Logic to delete the selected deck
        }

        private async void OnDeckSelected(IDeck selectedDeck)
        {
            if (selectedDeck != null)
            {
                try
                {
                    Console.WriteLine($"Fetching cards for deck: {selectedDeck.Name}");

                    //Attempt to fetch the cards for the selected deck                    
                    var deckCards = await selectedDeck.GetCards();

                    if (deckCards != null && deckCards.Count > 0)
                    {
                        // Update the SelectedDeckCards collection with the deck's card
                        SelectedDecksCards = new ObservableCollection<ICard>((IEnumerable<ICard>)deckCards);
                        Console.WriteLine($"Loaded {deckCards.Count} cards for deck: {selectedDeck.Name}");
                    }
                    else
                    {
                        // No cards found, clear the collection
                        SelectedDecksCards.Clear();
                        Console.WriteLine($"No cards found for deck: {selectedDeck.Name}");
                    }
                }
                catch (RestException)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to load deck cards. Please try again.", "OK");
                    }
                }
                catch (Exception)
                {
                    throw; // bubble up to the general exceptio handler
                }

            }
        }

        private void OnCreateDeckCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadUserDecks();
        }

        // Load the user's decks from the backend
        public async Task LoadUserDecks()
        {
            try
            {
                //Fetch the user decks from the backend service
                var userDecks = await _backendService.GetDecksAsync();

                //Check if the list of decks is null or empty
                if (userDecks != null && userDecks.Count > 0)
                {
                    UserDecks = new ObservableCollection<IDeck>(userDecks);
                    HasDecks = true;
                }
                else
                {
                    //No decks found
                    HasDecks = false;
                    UserDecks.Clear();
                }
            }
            catch (RestException)
            {
                HasDecks = false;

                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user decks. Please try again.", "OK");
                }
            }
            catch (Exception)
            {
                HasDecks = false;
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