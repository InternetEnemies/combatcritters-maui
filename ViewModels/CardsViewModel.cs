using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;
using UIKit;

namespace Combat_Critters_2._0.ViewModels
{
    public class CardsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ICard> _gameCards;
        private bool _hasCards;

        private readonly BackendService _backendService;
        public ICommand ReloadCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


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
            ReloadCommand = new Command(async () => await LoadGameCards());

            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
        }

        /// <summary>
        /// Loads game cards using predifined filter
        /// </summary>
        public async Task LoadGameCards()
        {
            IsLoading = true;
            bool hasCards = false; // function scoped variable
            try
            {
                var query = new CardQueryBuilder().Build();

                var cards = await _backendService.GetCardsAsync(query);

                if (cards != null && cards.Count > 0)
                {
                    GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                    hasCards = true;
                    Console.WriteLine($"Number of cards loaded: {GameCards.Count}");
                }
                else
                {
                    //Game has no Cards
                    GameCards.Clear();
                    Console.WriteLine("No cards found for the user.");
                }

            }
            catch (RestException)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user cards. Please try again.", "OK");
            }
            catch (Exception)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again later.", "OK");
            }
            finally
            {
                //Set HasCards based on result of operation
                HasCards = hasCards;
                IsLoading = false;
            }
        }

        //Properties for Adding Card a card to a vendor Offer
        private ObservableCollection<ICard> _selectedCardsforOffer;


        public ObservableCollection<ICard> SelectedCardsforOffer
        {
            get => _selectedCardsforOffer;
            set
            {
                _selectedCardsforOffer = value;
                OnPropertyChanged(nameof(SelectedCardsforOffer));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}