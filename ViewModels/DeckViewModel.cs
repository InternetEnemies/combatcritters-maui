using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;

namespace Combat_Critters_2._0.ViewModels
{
    public class DeckViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Deck> _userDecks;
        private ObservableCollection<Card> _selectedDeckCards; // Stores cards for the selected deck
        private Deck _selectedDeck;
        private bool _hasDecks = false;
        private bool _isDeckListVisible = false; // Controls the visibility of the dropdown menu

        public ObservableCollection<Deck> UserDecks
        {
            get => _userDecks;
            set
            {
                _userDecks = value;
                OnPropertyChanged(nameof(UserDecks));
            }
        }

        public ObservableCollection<Card> SelectedDeckCards
        {
            get => _selectedDeckCards;
            set
            {
                _selectedDeckCards = value;
                OnPropertyChanged(nameof(SelectedDeckCards));
            }
        }

        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                if (_selectedDeck != value)
                {
                    _selectedDeck = value;
                    OnPropertyChanged(nameof(SelectedDeck));

                    // Update the cards for the selected deck
                    if (_selectedDeck != null)
                    {
                        SelectedDeckCards = new ObservableCollection<Card>(_selectedDeck.Cards);
                        IsDeckListVisible = false; // Hide the dropdown after selection
                    }
                    else
                    {
                        SelectedDeckCards.Clear();
                    }
                }
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

        public bool IsDeckListVisible
        {
            get => _isDeckListVisible;
            set
            {
                _isDeckListVisible = value;
                OnPropertyChanged(nameof(IsDeckListVisible));
            }
        }

        // Command for toggling the dropdown menu visibility
        public ICommand ToggleDeckListCommand { get; }

        public DeckViewModel()
        {
            // Initialize _userDecks with an empty collection of Decks
            _userDecks = new ObservableCollection<Deck>();

            // Initialize _selectedDeckCards with an empty collection of Cards
            _selectedDeckCards = new ObservableCollection<Card>();

            // Initialize _selectedDeck with a default Deck (or use real values if needed)
            _selectedDeck = new Deck
            {
                Name = "Default Deck",
                Cards = new List<Card>() // Empty card list
            };
            ToggleDeckListCommand = new Command(ToggleDeckList);
            UserDecks = new ObservableCollection<Deck>();
            SelectedDeckCards = new ObservableCollection<Card>();
            LoadUserDecks(); // Load user decks
        }

        // Load the user's decks from the backend
        private async Task LoadUserDecks()
        {
            // // Example data for testing (replace with actual backend call)
            // var decks = await BackendService.GetUserDecksAsync();

            // if (decks != null && decks.Count > 0)
            // {
            //     UserDecks = new ObservableCollection<Deck>(decks);
            //     HasDecks = true;
            // }
            // else
            // {
            //     HasDecks = false; //User had no cards;
            // }
        }

        private void ToggleDeckList()
        {
            IsDeckListVisible = !IsDeckListVisible; // Toggle visibility
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}