using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;
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
        private ObservableCollection<Card> _selectedDeckCards; // Stores cards for the selected deck
        private IDeck _selectedDeck;
        private bool _hasDecks = false;
        private bool _isDeckListVisible = false; // Controls the visibility of the dropdown menu

        public ObservableCollection<IDeck> UserDecks
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

        public IDeck SelectedDeck
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
                        SelectedDeckCards = null;
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
            _userDecks = new ObservableCollection<IDeck>();

            // Initialize _selectedDeckCards with an empty collection of Cards
            _selectedDeckCards = new ObservableCollection<Card>();

            ToggleDeckListCommand = new Command(ToggleDeckList);
            UserDecks = new ObservableCollection<IDeck>();
            SelectedDeckCards = new ObservableCollection<Card>();

            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            
        }

        // Load the user's decks from the backend
        public async Task LoadUserDecks()
        {
            try
            {
                var userCards = await _backendService.GetDecksAsync();

                if (userCards == null)
                {
                    HasDecks = false; 
                }
                else
                {
                    UserDecks = new ObservableCollection<IDeck>(UserDecks);
                    HasDecks = true;
                }
            }
            catch(RestException ex)
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