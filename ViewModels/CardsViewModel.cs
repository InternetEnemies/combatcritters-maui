using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.managers;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.user;


namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ICard> _gameCards;
        private bool _hasCards; //Does a user have any card?

        private readonly BackendService _backendService;

        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
            }
        }
        public ObservableCollection<ICard> GameCards
        {
            get => _gameCards;
            set
            {
                _gameCards = value;
                OnPropertyChanged(nameof(GameCards));
            }
        }

        //Constructor
        public CardsViewModel()
        {
            _gameCards = new ObservableCollection<ICard>();
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            HasCards = false;

            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadUserCards();
        }

        public async Task LoadUserCards()
        {
            try
            {
                var cards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());

                if (cards != null && cards.Count > 0)
                {
                    HasCards = true;
                    GameCards = new ObservableCollection<ICard>((IEnumerable<ICard>)cards);
                }
                else
                {
                    //Game has no Cards
                    HasCards = false;

                    GameCards.Clear();

                }

            }
            catch (RestException)
            {
                HasCards = false;

                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards. Please try again.", "OK");
            }
            catch (Exception)
            {
                HasCards = false;

                throw; //bubble up to the global exception
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}