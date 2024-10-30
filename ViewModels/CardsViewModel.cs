using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.managers;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using CombatCrittersSharp.objects.user;
using UIKit;


namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ICard> _gameCards;
        private bool _hasCards; //Does a user have any card?

        private readonly BackendService _backendService;
        public ICommand ReloadCommand { get; }

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


            //Initialize Reload Command to reload the cards on button click LoadUserCards
            ReloadCommand = new Command(async () => await LoadUserCards());

            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadUserCards();
        }

        public async Task LoadUserCards()
        {
            bool hasCards = false; // function scoped variable
            try
            {
                var cards = await _backendService.GetCardsAsync(new CardQueryBuilder().Build());
                Console.WriteLine($"Received {cards.Count} cards from backend");
                if (cards != null && cards.Count > 0)
                {

                    Application.Current?.Dispatcher.Dispatch(() =>
                    {
                        GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                        hasCards = true;
                    });

                    Console.WriteLine($"Number of cards loaded: {GameCards.Count}");
                }
                else
                {
                    //Game has no Cards
                    GameCards.Clear();

                }

            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards. Please try again.", "OK");
            }

            finally
            {
                //Set HasCards based on result of operation
                HasCards = hasCards;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}