using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Models;

namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _userCards;
        private bool _hasCards; //Does a user have any card?

        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
            }
        }
        public ObservableCollection<Card> UserCards
        {
            get => _userCards;
            set
            {
                _userCards = value;
                OnPropertyChanged(nameof(UserCards));
            }
        }

        public CardsViewModel()
        {
            _userCards = [];
            LoadUserCards();
        }

        private async Task LoadUserCards()
        {

            //Call the BackendService to get the user's cards
            var cards = await BackendService.GetUserCardsAsync();

            //cards = null; //Test to see what happens if user has no cards
            //User has at least 1 card?
            if (cards != null && cards.Count > 0)
            {
                UserCards = new ObservableCollection<Card>(cards);
                HasCards = true;
            }
            else
            {
                HasCards = false; //User had no cards;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}