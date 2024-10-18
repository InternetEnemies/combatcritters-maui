using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Models;

namespace Combat_Critters_2._0.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Deck> _userDecks;
        public ICommand ShowDeckListCommand { get; set; } //Command for Deck List button
        private bool _hasDecks = false;
        private bool _deckListButtonClicked;

        private Deck _selectedDeck;

        private ObservableCollection<Card> _selectedDeckCards;


        public ObservableCollection<Deck> UserDecks
        {
            get => _userDecks;
            set
            {
                _userDecks = value;
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

        public bool DeckListButtonClicked
        {
            get => _deckListButtonClicked;
            set
            {
                _deckListButtonClicked = value;
                OnPropertyChanged(nameof(DeckListButtonClicked)); // Fixing the property name here
            }
        }
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set{
                _selectedDeck=value;
                OnPropertyChanged(nameof(SelectedDeck));

                //Update the cards from the selected deck
                if(_selectedDeck != null)
                {
                    SelectedDeckCards = new ObservableCollection<Card>(_selectedDeck.Cards);
                }
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

        public ProfileViewModel()
        {

            //Initialization
            _userDecks = new ObservableCollection<Deck>();
            // Initialize _selectedDeck with a required Name and an empty Cards collection
            _selectedDeck = new Deck
            {
                Name = " ",
                Cards = new List<Card>()  // Provide an empty collection for the cards
            };
            _selectedDeckCards = new ObservableCollection<Card>();

            DeckListButtonClicked = true; //Initially Set to true
            ShowDeckListCommand = new Command(OnDeckListButtonClicked); // Initialize the Button command
            LoadUserDeck(); // Load the user's deck list asynchronously
        }

        /*
            This method handles when the Deck list button is clicked
            The deck list becomes visible or hidden.
        */
        private void OnDeckListButtonClicked()
        {
            // Toggle the visibility of the deck list
            DeckListButtonClicked = !DeckListButtonClicked;
        }

        // This method retrieves the user deck list asynchronously
        private async Task LoadUserDeck()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
