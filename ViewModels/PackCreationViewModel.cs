using System.Collections.ObjectModel;
using System.ComponentModel;
using Combat_Critters_2._0.Services;
using CombatCrittersSharp.exception;
using CombatCrittersSharp.objects.card;
using CombatCrittersSharp.objects.card.Interfaces;

namespace Combat_Critters_2._0.ViewModels
{
    public class PackCreationViewModel : INotifyPropertyChanged
    {
        private readonly BackendService _backendService;

        public bool IsDraggable { get; set; } = true; // Control drag-and-drop functionality

        private bool _hasCards;
        public bool HasCards
        {
            get => _hasCards;
            set
            {
                _hasCards = value;
                OnPropertyChanged(nameof(HasCards));
            }
        }

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
        private ObservableCollection<ICard> _gameCards;
        public ObservableCollection<ICard> GameCards
        {
            get => _gameCards;
            set
            {
                _gameCards = value;
                OnPropertyChanged(nameof(GameCards));
            }
        }

        private string _packType;
        public string PackType
        {
            get => _packType;
            set
            {
                _packType = value;
                OnPropertyChanged(nameof(PackType));
                UpdateDisplay(); //Update UI based on pack type
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private int _cardLimit;
        public int CardLimit
        {
            get => _cardLimit;
            set
            {
                _cardLimit = value;
                OnPropertyChanged(nameof(CardLimit));
            }
        }


        public PackCreationViewModel(string packType)
        {
            _backendService = new BackendService(ClientSingleton.GetInstance("http://api.combatcritters.ca:4000"));
            _packType = packType;
            _description = "";
            _gameCards = new ObservableCollection<ICard>();
            HasCards = false;

            //LoadAvailableCards();
            UpdateDisplay();
            //start Loading the user cards.
            Task.Run(async () => await InitializeViewModelAsync());

        }

        private async Task InitializeViewModelAsync()
        {
            await LoadGameCards();
        }

        /// <summary>
        /// This method uses th backend services to load all game cards
        /// </summary>
        /// <returns>Task</returns>
        public async Task LoadGameCards()
        {
            IsLoading = true;
            bool hasCards = false; // function scoped variable
            try
            {
                CardQueryBuilder filteredBuild = new CardQueryBuilder();
                //filteredBuild.SetOwned(true);
                var cards = await _backendService.GetCardsAsync(filteredBuild.Build());
                Console.WriteLine($"Received {cards?.Count} cards from backend");
                if (cards != null && cards.Count > 0)
                {

                    // Application.Current?.Dispatcher.Dispatch(() =>
                    // 
                    GameCards = new ObservableCollection<ICard>(cards.Select(stack => stack.Item).ToList());
                    hasCards = true;
                    // });

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
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load game cards. Please try again.", "OK");
            }

            finally
            {
                //Set HasCards based on result of operation
                HasCards = hasCards;
                IsLoading = false;
            }
        }

        /// <summary>
        /// Update the display base on the pack type
        /// </summary>
        private void UpdateDisplay()
        {
            switch (PackType)
            {
                case "Basic":
                    Description = "A basic pack with limited selection of cards.";
                    CardLimit = 5;
                    break;

                case "Advanced":
                    Description = "An advanced pack";
                    CardLimit = 4;
                    break;

                case "Premium":
                    Description = "A premium pack";
                    CardLimit = 5;
                    break;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}