using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.deck;

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
            _hasDecks = false;

            //start Loading the user decks.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async void OnDeckSelected(IDeck selectedDeck)
        {
            if (selectedDeck != null)
            {
                try
                {
                    Console.WriteLine($"Fetching cards for deck: {selectedDeck.Name}");
                    var deckCards = await selectedDeck.GetCards();
                    if (deckCards != null)
                    {
                        SelectedDecksCards = new ObservableCollection<ICard>((IEnumerable<ICard>)deckCards);
                        Console.WriteLine($"Loaded {deckCards.Count} cards for deck: {selectedDeck.Name}");
                    }
                    else
                    {
                        SelectedDecksCards = new ObservableCollection<ICard>();
                        Console.WriteLine($"No cards found for deck: {selectedDeck.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading deck cards: {ex.Message}");
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
                var userDecks = await _backendService.GetDecksAsync();


                if (userDecks != null)
                {
                    UserDecks = new ObservableCollection<IDeck>(userDecks);
                    HasDecks = true;
                    Console.Write($"Had deck is {HasDecks}");
                }
                else
                {
                    Console.WriteLine("In here; fail");
                    //User has no decks
                    HasDecks = false;
                }
            }
            catch (RestException ex)
            {
                HasDecks = false;
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                HasDecks = false;
                Console.WriteLine($"General error occurred: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}