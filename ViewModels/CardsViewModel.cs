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

           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}