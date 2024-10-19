using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using Network;

namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService; 
        private ObservableCollection<ICard> _userCards;
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
        public ObservableCollection<ICard> UserCards
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
            _userCards = new ObservableCollection<ICard>();
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
        
            //start Loading the user cards.
            Task.Run(async () => await LoadUserCards());
        }
        
        /// <summary>
        /// This loads the card  page with user cards, if any
        /// </summary>
        /// <returns></returns>
        public async Task LoadUserCards()
        {
            try
            {
                //Default query for all cards
                var userCards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

                if (userCards == null)
                {
                    HasCards = false; 
                }
                else
                {
                    UserCards = new ObservableCollection<ICard>(userCards.Select(stack =>stack.Item));
                    HasCards = true;
                }
            }
            catch(RestException ex)
            {
                HasCards = false;
                Console.WriteLine(ex.Message);
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards from the server. Please try again.", "OK");
            }
            catch (Exception ex)
            {
                HasCards = false;
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